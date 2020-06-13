using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Delivery.API.Application.IntegrationEvents.EventHandling;
using Delivery.API.Application.IntegrationEvents.Events;
using Delivery.API.Grpc;
using Delivery.API.Infrastructure.AutofacModules;
using Delivery.API.Infrastructure.Filters;
using HealthChecks.UI.Client;
using Kangaroo.BuildingBlocks.EventBus.Abstractions;
using Kangaroo.Common.Facades;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Delivery.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }
        private IContainer Container { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
            services
                .AddOptions()
                .AddControllers(options => { options.Filters.Add(typeof(HttpGlobalExceptionFilter)); })
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
            container.RegisterType<DateTimeFacade>().As<IDateTimeFacade>();
            container.RegisterModule(new MediatorModule());
            container.RegisterModule(new ApplicationModule(Configuration["ConnectionString"]));
            Container = container.Build();
            return new AutofacServiceProvider(Container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IHostApplicationLifetime applicationLifetime
        )
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            app.UseRouting();
            app.UseAuthorization();
            app.UseCors("CorsPolicy");

            var pathBase = Configuration["PATH_BASE"];
            app.UseSwagger(c =>
                {
                    if (!string.IsNullOrWhiteSpace(pathBase))
                        c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                        {
                            swaggerDoc.Servers = new List<OpenApiServer> {new OpenApiServer {Url = $"{pathBase}"}};
                        });
                })
                .UseSwaggerUI(c => { c.SwaggerEndpoint($"{pathBase}/swagger/v1/swagger.json", "Delivery.API V1"); });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<ClientGrpcService>();
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
                // ~/hc returns full health check response with Self and SQL server database  state
                endpoints.MapHealthChecks("/hc", new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                // ~/liveness returns HealthStatus: "Healthy", "Unhealthy" or "Degraded"
                endpoints.MapHealthChecks("/liveness", new HealthCheckOptions
                {
                    Predicate = healthCheckRegistration => healthCheckRegistration.Name.Contains("self")
                });
            });
            //Disposes container on application stopped
            applicationLifetime.ApplicationStopped.Register(() => Container.Dispose());
            ConfigureEventBus(app);
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus
                .Subscribe<DeliveryStatusChangedToCourierPickedUpIntegrationEvent,
                    DeliveryStatusChangedToCourierPickedUpIntegrationEventHandler>();
            eventBus
                .Subscribe<CourierAssignedToDeliveryIntegrationEvent, CourierAssignedToDeliveryIntegrationEventHandler
                >();
        }
    }
}