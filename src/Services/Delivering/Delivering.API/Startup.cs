using Autofac;
using Autofac.Extensions.DependencyInjection;
using Delivering.API.Application.IntegrationEvents;
using Delivering.API.Application.IntegrationEvents.EventHandling;
using Delivering.API.Application.IntegrationEvents.Events;
using Delivering.API.Infrastructure.AutofacModules;
using Delivering.API.Infrastructure.Filters;
using Delivering.Infrastructure;
using HealthChecks.UI.Client;
using Kangaroo.BuildingBlocks.EventBus;
using Kangaroo.BuildingBlocks.EventBus.Abstractions;
using Kangaroo.BuildingBlocks.EventBusRabbitMQ;
using Kangaroo.BuildingBlocks.IntegrationEventLogEF;
using Kangaroo.BuildingBlocks.IntegrationEventLogEF.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using System;
using System.Data.Common;
using System.Reflection;

namespace Delivering.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IContainer Container { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services
                .AddOptions()
                .AddControllers(options =>
                {
                    options.Filters.Add(typeof(HttpGlobalExceptionFilter));
                })
                .AddNewtonsoftJson()
                .Services
                .AddHealthChecks(Configuration)
                .AddCorsPolicy()
                .AddSwagger()
                .AddEventBus(Configuration)
                .AddIntegrations(Configuration)
                .AddDbContext(Configuration);

            //configure autofac
            var container = new ContainerBuilder();
            container.Populate(services);
            container.RegisterModule(new MediatorModule());
            container.RegisterModule(new ApplicationModule(Configuration["ConnectionString"]));
            Container = container.Build();
            return new AutofacServiceProvider(Container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime applicationLifetime
            )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseCors("CorsPolicy");
            app.UseSwagger()
            .UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/v1/swagger.json", "Delivering.API V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
                // ~/hc returns full health check response with Self and SQL server database  state
                endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                // ~/liveness returns HealthStatus: "Healthy", "Unhealthy" or "Degraded"
                endpoints.MapHealthChecks("/liveness", new HealthCheckOptions()
                {
                    Predicate = healthCheckRegistration => healthCheckRegistration.Name.Contains("self")
                });
            });
            //Disposes container on application stopped
            applicationLifetime.ApplicationStopped.Register(() => Container.Dispose());
            ConfigureEventBus(app);
        }

        void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<DeliveryStatusChangedToCourierPickedUpIntegrationEvent, DeliveryStatusChangedToCourierPickedUpIntegrationEventHandler>();
        }
    }

    static class ExtensionsMethods
    {
        public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            var healthCheckBuilder = services.AddHealthChecks();

            healthCheckBuilder.AddCheck("self", () => HealthCheckResult.Healthy());

            healthCheckBuilder.AddSqlServer(
                configuration["ConnectionString"],
                name: "DeliveringDB-check",
                tags: new string[] { "deliveringdb" });

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Kangaroo - Delivering HTTP API",
                    Version = "v1",
                    Description = "The Delivering Service HTTP API"
                });
            });
            return services;
        }

        public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy => policy
                    .SetIsOriginAllowed((host) => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
            return services;
        }

        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEntityFrameworkSqlServer()
                .AddDbContext<DeliveringContext>(options =>
                {
                    options.UseSqlServer(configuration["ConnectionString"],
                        sqlServerOptionsAction: sqlOptions =>
                        {
                            sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                            sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                        });
                },
                    ServiceLifetime.Scoped  //Showing explicitly that the DbContext is shared across the HTTP request scope (graph of objects started in the HTTP request)
                );

            services.AddDbContext<IntegrationEventLogContext>(options =>
            {
                options.UseSqlServer(configuration["ConnectionString"],
                                     sqlServerOptionsAction: sqlOptions =>
                                     {
                                         sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                                         //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                                         sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                                     });
            });

            return services;
        }

        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            var subscriptionClientName = configuration["SubscriptionClientName"];
            services.AddSingleton<IEventBus, EventBusRabbitMQ>(serviceProvider =>
            {
                var rabbitMQPersistentConnection = serviceProvider.GetRequiredService<IRabbitMQPersistentConnection>();
                var iLifetimeScope = serviceProvider.GetRequiredService<ILifetimeScope>();
                var logger = serviceProvider.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                var eventBusSubcriptionsManager = serviceProvider.GetRequiredService<IEventBusSubscriptionsManager>();

                var retryCount = 5;
                if (!string.IsNullOrEmpty(configuration["EventBusRetryCount"]))
                {
                    retryCount = int.Parse(configuration["EventBusRetryCount"]);
                }

                return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, iLifetimeScope, eventBusSubcriptionsManager, subscriptionClientName, retryCount);
            });
            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
            return services;
        }

        public static IServiceCollection AddIntegrations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<Func<DbConnection, IIntegrationEventLogService>>(
                sp => (DbConnection c) => new IntegrationEventLogService(c));

            services.AddTransient<IDeliveringIntegrationEventService, DeliveringIntegrationEventService>();
            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();
                var factory = new ConnectionFactory()
                {
                    HostName = configuration["EventBusConnection"],
                    DispatchConsumersAsync = true
                };

                if (!string.IsNullOrEmpty(configuration["EventBusUserName"]))
                {
                    factory.UserName = configuration["EventBusUserName"];
                }

                if (!string.IsNullOrEmpty(configuration["EventBusPassword"]))
                {
                    factory.Password = configuration["EventBusPassword"];
                }

                var retryCount = 5;
                if (!string.IsNullOrEmpty(configuration["EventBusRetryCount"]))
                {
                    retryCount = int.Parse(configuration["EventBusRetryCount"]);
                }

                return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
            });
            return services;
        }
    }
}
