using System.Net;
using System.Threading.Tasks;
using Courier.API.DataTransferableObjects;
using Courier.API.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Courier.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CourierLocationsController : ControllerBase
    {
        private readonly ICourierLocationService _courierLocationService;

        public CourierLocationsController(ICourierLocationService courierLocationService)
        {
            _courierLocationService = courierLocationService;
        }

        [HttpPost]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.Accepted)]
        public async Task<IActionResult> CreateAsync(CourierLocationDtoSave courierLocation)
        {
            await _courierLocationService.InsertCourierLocationAsync(courierLocation);
            return Accepted();
        }
    }
}