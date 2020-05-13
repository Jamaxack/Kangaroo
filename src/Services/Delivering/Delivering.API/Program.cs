using Delivering.API.Infrastructure;
using Delivering.Infrastructure;
using Kangaroo.BuildingBlocks.IntegrationEventLogEF;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;

namespace Delivering.API
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
                logger.Debug("Init Delivering service");
                var host = WebHost.CreateDefaultBuilder(args)
                   .UseStartup<Startup>()
                   .ConfigureLogging(logging =>
                   {
                       logging.ClearProviders();
                       logging.SetMinimumLevel(LogLevel.Trace);
                   })
                   .UseNLog()  // NLog: Setup NLog for Dependency injection
                   .Build();

                host.MigrateDbContext<DeliveringContext>((context, services) =>
                {
                    var webHostEnvironment = services.GetService<IWebHostEnvironment>();
                    new DeliveringContextSeed()
                        .SeedAsync(context, webHostEnvironment)
                        .Wait();
                })
               .MigrateDbContext<IntegrationEventLogContext>((_, __) => { });
               
                host.Run();
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Stopped Delivering service because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }
    }
}
