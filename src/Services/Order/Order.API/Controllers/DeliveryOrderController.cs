using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Order.API.Application.Commands;
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
        [ProducesResponseType(typeof(DeliveryOrderViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetOrderByIdAsync(Guid orderId)
        {
            var deliveryOrder = await _deliveryOrderQueries.GetDeliveryOrderByIdAsync(orderId);
            return Ok(deliveryOrder);
        }  

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateOrderAsync([FromBody]CreateDeliveryOrderCommand createDeliveryOrderCommand, [FromHeader(Name = "x-requestid")] Guid requestId)
        {
            bool commandResult = false;
            if (requestId != Guid.Empty)
            {
                var requestCreateDeliveryOrder = new IdentifiedCommand<CreateDeliveryOrderCommand, bool>(createDeliveryOrderCommand, requestId);
               
                _logger.LogInformation(
                    "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                    requestCreateDeliveryOrder.GetGenericTypeName(),
                    nameof(requestCreateDeliveryOrder.Command.ClientId),
                    requestCreateDeliveryOrder.Command.ClientId,
                    requestCreateDeliveryOrder);

                commandResult = await _mediator.Send(requestCreateDeliveryOrder);
            }

            if (!commandResult)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
