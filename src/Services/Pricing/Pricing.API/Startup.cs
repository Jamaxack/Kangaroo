using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pricing.API.Infrastucture.Filters;
using Pricing.API.Validators;

namespace Pricing.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers(options => options.Filters.Add(typeof(HttpGlobalExceptionFilter)))
                .AddNewtonsoftJson()
                .AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining<IFluentValidator>())
                .Services
                .AddOptions()
                .AddHttpClient()
                .Configure<PricingSettings>(Configuration)
                .AddHealthCheck(Configuration)
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
            app.UseSwagger(Configuration["PATH_BASE"]);
        }
    }
}