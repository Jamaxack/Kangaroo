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

    [Route("api/v1/[controller]")]
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

        [Route("{courierId}")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Courier), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetByIdAsync(Guid courierId)
            => Ok(await _courierService.GetCourierByIdAsync(courierId));

        [Route("{courierId}/Deliveries")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(List<Courier>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDeliveriesByCourierIdAsync(Guid courierId)
           => Ok(await _courierService.GetDeliveriesByCourierIdAsync(courierId));

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        public async Task<IActionResult> CreateAsync(Courier courier)
        {
            await _courierService.InsertCourierAsync(courier);
            return Accepted();
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        public async Task<IActionResult> UpdateAsync(Courier courier)
        {
            await _courierService.UpdateCourierAsync(courier);
            return Accepted();
        }

        [Route("{courierId}/CurrentLocation")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(CourierLocation), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCurrectLocationByCourierIdAsync(Guid courierId)
        {
            return Ok(await _courierService.GetCurrentCourierLocationByCourierIdAsync(courierId));
        }
    }
}
