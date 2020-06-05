using Delivery.API.Infrastructure;
using Delivery.Infrastructure;
using Kangaroo.BuildingBlocks.IntegrationEventLogEF;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;
using System.IO;
using System.Net;
using System.Reflection;

namespace Delivery.API
{
    public class Program
    {
        public static readonly string Namespace = typeof(Program).Namespace;
        public static readonly string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);

        public static int Main(string[] args)
        {
            try
            {
                var configuration = GetConfiguration();
                ConfigureLogging(configuration);
                Log.Information("Configuring web host ({ApplicationContext})...", AppName);
                var host = WebHost.CreateDefaultBuilder(args)
                   .UseStartup<Startup>()
                   .ConfigureKestrel(options =>
                   {
                       var (httpPort, grpcPort) = GetDefinedPorts(configuration);
                       options.Listen(IPAddress.Any, httpPort, listenOptions =>
                       {
                           listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                       });
                       options.Listen(IPAddress.Any, grpcPort, listenOptions =>
                       {
                           listenOptions.Protocols = HttpProtocols.Http2;
                       });

                   })
                   .UseSerilog()
                   .Build();

                host
                    .MigrateDbContext<DeliveryContext>((context, services) =>
                    {
                        var webHostEnvironment = services.GetService<IWebHostEnvironment>();
                        new DeliveryContextSeed().SeedAsync(context, webHostEnvironment).Wait();
                    })
                    .MigrateDbContext<IntegrationEventLogContext>((_, __) => { });

                Log.Information("Starting web host ({ApplicationContext})...", AppName);
                host.Run();

                return 0;
            }
            catch (Exception exception)
            {
                Log.Fatal(exception, "Program terminated unexpectedly ({ApplicationContext})", AppName);
                return 1;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                Log.CloseAndFlush();
            }
        }

        static IConfigurationRoot GetConfiguration()
            => new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        static (int httpPort, int grpcPort) GetDefinedPorts(IConfigurationRoot configuration)
        {
            var grpcPort = configuration.GetValue("GRPC_PORT", 5001);
            var port = configuration.GetValue("PORT", 5000);
            return (port, grpcPort);
        }

        static void ConfigureLogging(IConfigurationRoot configuration)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.Elasticsearch(ConfigureElasticSink(configuration, environment))
                .Enrich.WithProperty("ApplicationContext", AppName)
                .Enrich.WithProperty("Environment", environment)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }

        static ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot configuration, string environment)
            => new ElasticsearchSinkOptions(new Uri(configuration.GetValue("ElasticUri", "http://localhost:9200")))
            {
                AutoRegisterTemplate = true,
                IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-")}-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
            };
    }
}
