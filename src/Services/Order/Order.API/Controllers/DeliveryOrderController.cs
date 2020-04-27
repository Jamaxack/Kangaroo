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

        //GET api/v1/[controller]/5f293549-65c4-4332-a88a-853b4dd937f0
        [Route("{orderId:Guid}")]
        [HttpGet]
        [ProducesResponseType(typeof(DeliveryOrderViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetOrderByIdAsync(Guid orderId)
        {
            try
            {
                var deliveryOrder = await _deliveryOrderQueries.GetDeliveryOrderByIdAsync(orderId);
                return Ok(deliveryOrder);
            }
            catch
            {
                return NotFound();
            }
        }

        //GET api/v1/[controller]/ByClientId/5f293549-65c4-4332-a88a-853b4dd937f0
        [Route("ByClientId/{clientId:Guid}")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DeliveryOrderViewModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetOrderByClientIdAsync(Guid clientId)
        {
            try
            {
                var deliveryOrders = await _deliveryOrderQueries.GetDeliveryOrdersByClientIdAsync(clientId);
                return Ok(deliveryOrders);
            }
            catch
            {
                return NotFound();
            }
        }

        //GET api/v1/[controller]/ByCourierId/5f293549-65c4-4332-a88a-853b4dd937f0
        [Route("ByCourierId/{courierId:Guid}")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DeliveryOrderViewModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetOrderByCourierIdAsync(Guid courierId)
        {
            try
            {
                var deliveryOrders = await _deliveryOrderQueries.GetDeliveryOrdersByCourierIdAsync(courierId);
                return Ok(deliveryOrders);
            }
            catch
            {
                return NotFound();
            }
        }

        //GET api/v1/[controller][?pageSize=12&pageIndex=7]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DeliveryOrderViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetOrdersAsync([FromQuery]int pageSize = 10, [FromQuery]int pageIndex = 0)
        {
            var deliveryOrders = await _deliveryOrderQueries.GetDeliveryOrdersAsync(pageSize, pageIndex);
            return Ok(deliveryOrders);
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

        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)] 
        public async Task<IActionResult> DeleteDeliveryLocationAsync([FromBody]DeleteDeliveryLocationCommand deleteDeliveryLocationCommand, [FromHeader(Name = "x-requestid")] Guid requestId)
        {
            bool commandResult = false;
            if (requestId != Guid.Empty)
            {
                var requestDeleteDeliveryLocation = new IdentifiedCommand<DeleteDeliveryLocationCommand, bool>(deleteDeliveryLocationCommand, requestId);

                _logger.LogInformation(
                    "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                    requestDeleteDeliveryLocation.GetGenericTypeName(),
                    nameof(requestDeleteDeliveryLocation.Command.DeliveryLocationId),
                    requestDeleteDeliveryLocation.Command.DeliveryLocationId,
                    requestDeleteDeliveryLocation);

                commandResult = await _mediator.Send(requestDeleteDeliveryLocation);
            }

            if (!commandResult)
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}
