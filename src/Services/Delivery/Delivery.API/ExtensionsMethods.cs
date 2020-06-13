using System;
using System.Data.Common;
using System.Reflection;
using Autofac;
using Delivery.API.Application.IntegrationEvents;
using Delivery.Infrastructure;
using Kangaroo.BuildingBlocks.EventBus;
using Kangaroo.BuildingBlocks.EventBus.Abstractions;
using Kangaroo.BuildingBlocks.EventBusRabbitMQ;
using Kangaroo.BuildingBlocks.IntegrationEventLogEF;
using Kangaroo.BuildingBlocks.IntegrationEventLogEF.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;

namespace Delivery.API
{
    internal static class ExtensionsMethods
    {
        public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            var healthCheckBuilder = services.AddHealthChecks();

            healthCheckBuilder.AddCheck("self", () => HealthCheckResult.Healthy());

            healthCheckBuilder.AddSqlServer(
                configuration["ConnectionString"],
                name: "DeliveryDB-check",
                tags: new[] {"deliverydb"});

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Kangaroo - Delivery HTTP API",
                    Version = "v1",
                    Description = "The Delivery Service HTTP API"
                });
            });
            return services;
        }

        public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy => policy
                    .SetIsOriginAllowed(host => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
            return services;
        }

        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEntityFrameworkSqlServer()
                .AddDbContext<DeliveryContext>(options =>
                    {
                        options.UseSqlServer(configuration["ConnectionString"],
                            sqlOptions =>
                            {
                                sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                                sqlOptions.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), null);
                            });
                    } //Showing explicitly that the DbContext is shared across the HTTP request scope (graph of objects started in the HTTP request)
                );

            services.AddDbContext<IntegrationEventLogContext>(options =>
            {
                options.UseSqlServer(configuration["ConnectionString"],
                    sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                        //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                        sqlOptions.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), null);
                    });
            });

            return services;
        }

        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            var subscriptionClientName = configuration["SubscriptionClientName"];
            services.AddSingleton<IEventBus, EventBusRabbitMQ>(serviceProvider =>
            {
                var rabbitMqPersistentConnection = serviceProvider.GetRequiredService<IRabbitMQPersistentConnection>();
                var iLifetimeScope = serviceProvider.GetRequiredService<ILifetimeScope>();
                var logger = serviceProvider.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                var eventBusSubscriptionsManager = serviceProvider.GetRequiredService<IEventBusSubscriptionsManager>();

                var retryCount = 5;
                if (!string.IsNullOrEmpty(configuration["EventBusRetryCount"]))
                    retryCount = int.Parse(configuration["EventBusRetryCount"]);

                return new EventBusRabbitMQ(rabbitMqPersistentConnection, logger, iLifetimeScope,
                    eventBusSubscriptionsManager, subscriptionClientName, retryCount);
            });
            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
            return services;
        }

        public static IServiceCollection AddIntegrations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<Func<DbConnection, IIntegrationEventLogService>>(
                sp => c => new IntegrationEventLogService(c));

            services.AddTransient<IDeliveryIntegrationEventService, DeliveryIntegrationEventService>();
            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();
                var factory = new ConnectionFactory
                {
                    HostName = configuration["EventBusConnection"],
                    DispatchConsumersAsync = true
                };

                if (!string.IsNullOrEmpty(configuration["EventBusUserName"]))
                    factory.UserName = configuration["EventBusUserName"];

                if (!string.IsNullOrEmpty(configuration["EventBusPassword"]))
                    factory.Password = configuration["EventBusPassword"];

                var retryCount = 5;
                if (!string.IsNullOrEmpty(configuration["EventBusRetryCount"]))
                    retryCount = int.Parse(configuration["EventBusRetryCount"]);

                return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
            });
            return services;
        }
    }
}