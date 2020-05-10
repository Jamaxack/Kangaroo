using Courier.API.DataTransferableObjects;
using Courier.API.Infrastructure.Services;
using Courier.API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Courier.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CourierLocationsController : ControllerBase
    {
        readonly ICourierLocationService _courierLocationService;
        readonly ILogger<CourierLocationsController> _logger;

        public CourierLocationsController(ICourierLocationService courierLocationService, ILogger<CourierLocationsController> logger)
        {
            _courierLocationService = courierLocationService;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        public async Task<IActionResult> CreateAsync(CourierLocationDTO courierLocation)
        {
            await _courierLocationService.InsertCourierLocationAsync(courierLocation);
            return Accepted();
        } 
    }
}
