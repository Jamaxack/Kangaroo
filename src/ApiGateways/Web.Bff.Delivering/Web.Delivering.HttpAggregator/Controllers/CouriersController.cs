using GrpcCourier;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Web.Delivering.HttpAggregator.Models;
using Web.Delivering.HttpAggregator.Services;

namespace Web.Delivering.HttpAggregator.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]

    public class CouriersController : ControllerBase
    {
        private readonly ICourierService _courierService;

        public CouriersController(ICourierService courierService)
        {
            _courierService = courierService;
        }

        [Route("{deliveryId}")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Delivery), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDeliveryByIdAsync(string deliveryId)
           => Ok(await _courierService.GetDeliveryByIdAsync(deliveryId));

    }
}
