using Delivery.API.Infrastructure;
using Delivery.Infrastructure;
using Kangaroo.BuildingBlocks.IntegrationEventLogEF;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;
using System.IO;
using System.Net;

namespace Delivery.API
{
    public class Program
    {
        public static readonly string Namespace = typeof(Program).Namespace;
        public static readonly string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);

        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("Init Delivery service");
                var host = WebHost.CreateDefaultBuilder(args)
                   .UseStartup<Startup>()
                   .ConfigureKestrel(options =>
                    {
                        var (httpPort, grpcPort) = GetDefinedPorts();
                        options.Listen(IPAddress.Any, httpPort, listenOptions =>
                        {
                            listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                        });
                        options.Listen(IPAddress.Any, grpcPort, listenOptions =>
                        {
                            listenOptions.Protocols = HttpProtocols.Http2;
                        });

                    })
                   .ConfigureLogging(logging =>
                   {
                       logging.ClearProviders();
                       logging.SetMinimumLevel(LogLevel.Trace);
                   })
                   .UseNLog()  // NLog: Setup NLog for Dependency injection
                   .Build();

                host.MigrateDbContext<DeliveryContext>((context, services) =>
                {
                    var webHostEnvironment = services.GetService<IWebHostEnvironment>();
                    new DeliveryContextSeed()
                        .SeedAsync(context, webHostEnvironment)
                        .Wait();
                })
               .MigrateDbContext<IntegrationEventLogContext>((_, __) => { });

                host.Run();
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Stopped Delivery service because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }

        static (int httpPort, int grpcPort) GetDefinedPorts()
        {
            var configuration = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
              .AddEnvironmentVariables().Build();

            var grpcPort = configuration.GetValue("GRPC_PORT", 5001);
            var port = configuration.GetValue("PORT", 5000);
            return (port, grpcPort);
        }
    }
}
