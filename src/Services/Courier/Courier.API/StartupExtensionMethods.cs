using Autofac;
using Courier.API.Infrastructure.Repositories;
using Courier.API.Infrastructure.Services;
using Courier.API.IntegrationEvents.EventHandling;
using Kangaroo.BuildingBlocks.EventBus;
using Kangaroo.BuildingBlocks.EventBus.Abstractions;
using Kangaroo.BuildingBlocks.EventBusRabbitMQ;
using Kangaroo.Common.Facades;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;

namespace Courier.API
{
    public static class StartupExtensionMethods
    {
        public static IServiceCollection AddHealthCheck(this IServiceCollection services, IConfiguration configuration)
        {
            var hcBuilder = services.AddHealthChecks();

            hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy());

            hcBuilder
                .AddMongoDb(
                    configuration["ConnectionString"],
                    name: "couriers-mongodb-check",
                    tags: new[] {"mongodb"});

            return services;
        }

        public static IServiceCollection RegisterEventBus(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<IRabbitMQPersistentConnection>(serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

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
            services.AddTransient<DeliveryCreatedIntegrationEventHandler>();

            return services;
        }

        public static IServiceCollection AddDependencyInjections(this IServiceCollection services)
        {
            services.AddTransient<ICourierService, CourierService>();
            services.AddTransient<ICourierLocationService, CourierLocationService>();
            services.AddTransient<IDeliveryService, DeliveryService>();
            services.AddTransient<ICourierRepository, CourierRepository>();
            services.AddTransient<ICourierLocationRepository, CourierLocationRepository>();
            services.AddTransient<IClientRepository, ClientRepository>();
            services.AddTransient<IDeliveryRepository, DeliveryRepository>();
            services.AddTransient<IDateTimeFacade, DateTimeFacade>();
            return services;
        }

        public static IServiceCollection AddCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .SetIsOriginAllowed(host => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });
            return services;
        }

        public static IServiceCollection AddSwaggerGen(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Kangaroo - Courier HTTP API",
                    Version = "v1",
                    Description = "The Courier Microservice HTTP API"
                });
            });
            return services;
        }
    }
}