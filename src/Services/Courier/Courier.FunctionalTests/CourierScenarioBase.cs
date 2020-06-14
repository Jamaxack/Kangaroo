using Courier.API;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;

namespace Courier.FunctionalTests
{
    public class CourierScenarioBase
    {
        public TestServer CreateTestServer()
        {
            var path = Assembly.GetAssembly(typeof(CourierScenarioBase)).Location;

            var hostBuilder = new WebHostBuilder()
                .UseContentRoot(Path.GetDirectoryName(path))
                .ConfigureAppConfiguration(contextBuilder =>
                {
                    contextBuilder.AddJsonFile("appsettings.json", optional: false).AddEnvironmentVariables();
                })
                .UseStartup<Startup>();

            return new TestServer(hostBuilder);
        }

        public static class Get
        {
            public static string Couriers = "api/v1/couriers"; 
        } 
    }
}
