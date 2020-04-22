using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Order.API.Application.Queries;

namespace Order.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DeliveryOrderController : ControllerBase
    {
        readonly IMediator _mediator;
        readonly ILogger<DeliveryOrderController> _logger;
        readonly IDeliveryOrderQueries _deliveryOrderQueries;

        public DeliveryOrderController(
            IMediator mediator,
            IDeliveryOrderQueries deliveryOrderQueries,
            ILogger<DeliveryOrderController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _deliveryOrderQueries = deliveryOrderQueries;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderAsync(Guid orderId)
        {
            var deliveryOrder = await _deliveryOrderQueries.GetDeliveryOrderAsync(orderId);
            return Ok(deliveryOrder);
        }
    }
}
