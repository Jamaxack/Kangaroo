using Courier.API.DataTransferableObjects;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
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
                var response = await server.CreateClient().GetAsync(BaseApiUri);
                response.EnsureSuccessStatusCode();
            }
        }

        [Fact]
        public async Task Couriers_post_should_response_ok_status_code()
        {
            using (var server = CreateTestServer())
            {
                var contentCourier = new StringContent(BuildCourier(), UTF8Encoding.UTF8, "application/json");
                var response = await server.CreateClient().PostAsync(BaseApiUri, contentCourier);

                response.EnsureSuccessStatusCode();
            }
        }

        private string BuildCourier()
        {
            var courierDto = new CourierDtoSave
            {
                FirstName = "Albert",
                LastName = "Einstein",
                Phone = "7777777"
            };
            return JsonConvert.SerializeObject(courierDto);
        }
    }
}
