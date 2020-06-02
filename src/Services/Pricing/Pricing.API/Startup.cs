using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Pricing.API.Infrastucture.Filters;
using Pricing.API.Infrastucture.Repositories;
using Pricing.API.Infrastucture.Services;
using Pricing.API.Validators;
using StackExchange.Redis;
using System.Collections.Generic;

namespace Pricing.API
{
    public class Startup
    {
        IConfiguration _configuration { get; }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers(options => options.Filters.Add(typeof(HttpGlobalExceptionFilter)))
                .AddNewtonsoftJson()
                .AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining<IFluentValidator>())
                .Services
                .AddOptions()
                .AddHttpClient()
                .Configure<PricingSettings>(_configuration)
                .AddHealthCheck(_configuration)
                .AddSwaggerGen()
                .AddCors()
                .AddDependencyInjections();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment webHostEnvironment)
        {
            if (webHostEnvironment.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app
                .UseCors("CorsPolicy")
                .UseRouting()
                .UseAuthorization()
                .UseEndpoints();
            app.UseSwagger(_configuration["PATH_BASE"]);
        }
    }

    public static class StartupExtensionMethods
    {
        public static void UseEndpoints(this IApplicationBuilder app)
            =>
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapHealthChecks("/liveness", new HealthCheckOptions
                {
                    Predicate = r => r.Name.Contains("self")
                });
            });

        public static void UseSwagger(this IApplicationBuilder app, string pathBase)
            =>
            app.UseSwagger(c =>
            {
                if (!string.IsNullOrWhiteSpace(pathBase))
                {
                    c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                    {
                        swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{pathBase}" } };
                    });
                }
            })
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"{pathBase}/swagger/v1/swagger.json", "Pricing.API V1");
                    c.OAuthClientId("pricingswaggerui");
                    c.OAuthAppName("Pricing Swagger UI");
                });

        public static IServiceCollection AddHealthCheck(this IServiceCollection services, IConfiguration configuration)
            => services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy())
            .AddRedis(
                configuration["ConnectionString"],
                name: "redis-check",
                tags: new string[] { "redis" })
            .Services;

        public static IServiceCollection AddDependencyInjections(this IServiceCollection services)
            => services
            .AddSingleton<ConnectionMultiplexer>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<PricingSettings>>().Value;
                var configuration = ConfigurationOptions.Parse(settings.ConnectionString, true);
                configuration.ResolveDns = true;
                return ConnectionMultiplexer.Connect(configuration);
            })
            .AddTransient<IPricingRepository, RedisPricingRepository>()
            .AddTransient<IPricingService, PricingService>();

        public static IServiceCollection AddCors(this IServiceCollection services)
            => services.AddCors(options =>
             {
                 options.AddPolicy("CorsPolicy",
                     builder => builder
                     .SetIsOriginAllowed((host) => true)
                     .AllowAnyMethod()
                     .AllowAnyHeader()
                     .AllowCredentials());
             });

        public static IServiceCollection AddSwaggerGen(this IServiceCollection services)
            => services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Kangaroo - Pricing HTTP API",
                    Version = "v1",
                    Description = "The Pricing Microservice HTTP API",
                });
            });
    }
}
