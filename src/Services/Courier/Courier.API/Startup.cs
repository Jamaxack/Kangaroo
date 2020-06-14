using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Courier.API.Configurations;
using Courier.API.Infrastructure;
using Courier.API.Infrastructure.Filters;
using Courier.API.Infrastructure.GrpcServices;
using Courier.API.IntegrationEvents.EventHandling;
using Courier.API.IntegrationEvents.Events;
using Courier.API.Mapping;
using Courier.API.Validators;
using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using Kangaroo.BuildingBlocks.EventBus.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Courier.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient<IClientGrpcService, ClientGrpcService>();
            services.AddControllers(options => options.Filters.Add(typeof(HttpGlobalExceptionFilter)))
                .AddNewtonsoftJson()
                .AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining<IFluentValidator>())
                .Services
                .AddHealthCheck(Configuration)
                .AddOptions()
                .Configure<CourierSettings>(Configuration)
                .Configure<UrlsConfiguration>(Configuration.GetSection("urls"))
                .RegisterEventBus(Configuration)
                .AddSwaggerGen()
                .AddCors()
                .AddDependencyInjections()
                .AddAutoMapper(typeof(MappingProfile));

            //configure autofac
            var container = new ContainerBuilder();
            container.Populate(services);
            return new AutofacServiceProvider(container.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseCors("CorsPolicy");
            app.UseRouting();
            app.UseAuthorization();
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

            var pathBase = Configuration["PATH_BASE"];
            app.UseSwagger(c =>
                {
                    if (!string.IsNullOrWhiteSpace(pathBase))
                        c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                        {
                            swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{pathBase}" } };
                        });
                })
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"{pathBase}/swagger/v1/swagger.json", "Couriers.API V1");
                    c.OAuthClientId("couriersswaggerui");
                    c.OAuthAppName("Couriers Swagger UI");
                });

            CourierContextSeed.SeedAsync(app, loggerFactory).Wait();
            ConfigureEventBus(app);
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<DeliveryCreatedIntegrationEvent, DeliveryCreatedIntegrationEventHandler>();
        }
    }
}