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
        {
            var couriers = await _courierService.GetCouriersAsync();
            return Ok(couriers);
        }
    }
}
