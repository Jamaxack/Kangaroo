namespace Courier.API.Controllers
{
    using Courier.API.Infrastructure.Services;
    using Courier.API.Model;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;

    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class CouriersController : ControllerBase
    {
        readonly ICourierService _courierService;
        readonly ILogger<CouriersController> _logger;

        public CouriersController(ICourierService courierService, ILogger<CouriersController> logger)
        {
            _courierService = courierService ?? throw new ArgumentNullException(nameof(courierService)); ;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); ;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Courier>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAsync()
            => Ok(await _courierService.GetCouriersAsync());

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Courier), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetByIdAsync(Guid courierId)
            => Ok(await _courierService.GetCourierByIdAsync(courierId));

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)] 
        [ProducesResponseType(typeof(List<Courier>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDeliveriesByCourierIdAsync(Guid courierId)
           => Ok(await _courierService.GetDeliveriesByCourierIdAsync(courierId));

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        public async Task<IActionResult> CreateCourierAsync(Courier courier)
        {
            await _courierService.InsertCourierAsync(courier);
            return Accepted();
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        public async Task<IActionResult> UpdateCourierAsync(Courier courier)
        {
            await _courierService.UpdateCourierAsync(courier);
            return Accepted();
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        public async Task<IActionResult> CreateCourierLocationAsync(CourierLocation courierLocation)
        {
            await _courierService.InsertCourierLocationAsync(courierLocation);
            return Accepted();
        }


        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(CourierLocation), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCurrectLocationByIdAsync(Guid courierId)
        {
           return Ok(await _courierService.GetCurrentCourierLocationByCourierIdAsync(courierId));
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        public async Task<IActionResult> CreateDeliveryAsync(Delivery delivery)
        {
            await _courierService.InsertDeliveryAsync(delivery);
            return Accepted();
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Delivery), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDeliveryByIdAsync(Guid deliveryId)
            => Ok(await _courierService.GetDeliveryByIdAsync(deliveryId));


        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        public async Task<IActionResult> DeleteDeliveryByIdAsync(Guid deliveryId)
        {
            await _courierService.DeleteDeliveryByIdAsync(deliveryId);
            return Accepted();
        }
    }
}
