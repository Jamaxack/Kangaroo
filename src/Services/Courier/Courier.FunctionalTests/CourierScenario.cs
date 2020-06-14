using System.Threading.Tasks;
using Xunit;

namespace Courier.FunctionalTests
{
    public class CourierScenario : CourierScenarioBase
    {
        [Fact]
        public async Task Couriers_get_should_response_ok_status_code()
        {
            using (var server = CreateTestServer())
            {
                var response = await server.CreateClient().GetAsync(Get.Couriers);
                response.EnsureSuccessStatusCode();
            }
        }
    }
}
