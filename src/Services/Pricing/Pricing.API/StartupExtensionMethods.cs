using System.Collections.Generic;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Pricing.API.Infrastucture.Repositories;
using Pricing.API.Infrastucture.Services;
using StackExchange.Redis;

namespace Pricing.API
{
    public static class StartupExtensionMethods
    {
        public static void UseEndpoints(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/hc", new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapHealthChecks("/liveness", new HealthCheckOptions
                {
                    Predicate = r => r.Name.Contains("self")
                });
            });
        }

        public static void UseSwagger(this IApplicationBuilder app, string pathBase)
        {
            app.UseSwagger(c =>
                {
                    if (!string.IsNullOrWhiteSpace(pathBase))
                        c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                        {
                            swaggerDoc.Servers = new List<OpenApiServer> {new OpenApiServer {Url = $"{pathBase}"}};
                        });
                })
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"{pathBase}/swagger/v1/swagger.json", "Pricing.API V1");
                    c.OAuthClientId("pricingswaggerui");
                    c.OAuthAppName("Pricing Swagger UI");
                });
        }

        public static IServiceCollection AddHealthCheck(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy())
                .AddRedis(
                    configuration["ConnectionString"],
                    "redis-check",
                    tags: new[] {"redis"})
                .Services;
        }

        public static IServiceCollection AddDependencyInjections(this IServiceCollection services)
        {
            return services
                .AddSingleton(sp =>
                {
                    var settings = sp.GetRequiredService<IOptions<PricingSettings>>().Value;
                    var configuration = ConfigurationOptions.Parse(settings.ConnectionString, true);
                    configuration.ResolveDns = true;
                    return ConnectionMultiplexer.Connect(configuration);
                })
                .AddTransient<IPricingRepository, RedisPricingRepository>()
                .AddTransient<IPricingService, PricingService>();
        }

        public static IServiceCollection AddCors(this IServiceCollection services)
        {
            return services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .SetIsOriginAllowed(host => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });
        }

        public static IServiceCollection AddSwaggerGen(this IServiceCollection services)
        {
            return services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Kangaroo - Pricing HTTP API",
                    Version = "v1",
                    Description = "The Pricing Microservice HTTP API"
                });
            });
        }
    }
}