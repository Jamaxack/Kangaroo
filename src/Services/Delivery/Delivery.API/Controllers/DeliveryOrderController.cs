using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Delivery.API.Application.Commands;
using Delivery.API.Application.Queries;

namespace Delivery.API.Controllers
{
    [Route("api/v1/[controller]/[action]")]
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

        //GET ~/api/v1/[controller]/ByDeliveryOrderId/3fa85f64-5717-4562-b3fc-2c963f66afa6 
        [Route("{deliveryOrderId:Guid}")]
        [HttpGet]
        [ProducesResponseType(typeof(DeliveryOrderViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> ByDeliveryOrderIdAsync(Guid deliveryOrderId)
        {
            try
            {
                var deliveryOrder = await _deliveryOrderQueries.GetDeliveryOrderByIdAsync(deliveryOrderId);
                return Ok(deliveryOrder);
            }
            catch
            {
                return NotFound();
            }
        }

        //GET ~/api/v1/[controller]/ByClientId/3fa85f64-5717-4562-b3fc-2c963f66afa6 
        [Route("{clientId:Guid}")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DeliveryOrderViewModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> ByClientIdAsync(Guid clientId)
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

        //GET ~/api/v1/[controller]/ByCourierId/3fa85f64-5717-4562-b3fc-2c963f66afa6 
        [Route("{courierId:Guid}")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DeliveryOrderViewModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> ByCourierIdAsync(Guid courierId)
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

        //GET ~/api/v1/[controller]/Get[?pageSize=12&pageIndex=7]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DeliveryOrderViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAsync([FromQuery]int pageSize = 10, [FromQuery]int pageIndex = 0)
        {
            var deliveryOrders = await _deliveryOrderQueries.GetDeliveryOrdersAsync(pageSize, pageIndex);
            return Ok(deliveryOrders);
        }

        //POST ~/api/v1/[controller]/Create
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateAsync(CreateDeliveryOrderCommand createDeliveryOrderCommand, [FromHeader(Name = "x-requestid")] Guid requestId)
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

        //POST ~/api/v1/[controller]/SetStatusToAvailable
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SetStatusToAvailableAsync(SetAvailableDeliveryOrderStatusCommand setAvailableDeliveryOrderStatusCommand, [FromHeader(Name = "x-requestid")] Guid requestId)
        {
            bool commandResult = false;
            if (requestId != Guid.Empty)
            {
                var requestSetAvailableDeliveryOrderStatusCommand = new IdentifiedCommand<SetAvailableDeliveryOrderStatusCommand, bool>(setAvailableDeliveryOrderStatusCommand, requestId);

                _logger.LogInformation(
                    "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                    requestSetAvailableDeliveryOrderStatusCommand.GetGenericTypeName(),
                    nameof(requestSetAvailableDeliveryOrderStatusCommand.Command.DeliveryOrderId),
                    requestSetAvailableDeliveryOrderStatusCommand.Command.DeliveryOrderId,
                    requestSetAvailableDeliveryOrderStatusCommand);

                commandResult = await _mediator.Send(requestSetAvailableDeliveryOrderStatusCommand);
            }

            if (!commandResult)
            {
                return BadRequest();
            }

            return Ok();
        }

        //POST ~/api/v1/[controller]/Delete
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteAsync(DeleteDeliveryOrderCommand deleteDeliveryOrderCommand, [FromHeader(Name = "x-requestid")] Guid requestId)
        {
            bool commandResult = false;
            if (requestId != Guid.Empty)
            {
                var requestDeleteDeliveryOrder = new IdentifiedCommand<DeleteDeliveryOrderCommand, bool>(deleteDeliveryOrderCommand, requestId);

                _logger.LogInformation(
                    "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                    requestDeleteDeliveryOrder.GetGenericTypeName(),
                    nameof(requestDeleteDeliveryOrder.Command.DeliveryOrderId),
                    requestDeleteDeliveryOrder.Command.DeliveryOrderId,
                    requestDeleteDeliveryOrder);

                commandResult = await _mediator.Send(requestDeleteDeliveryOrder);
            }

            if (!commandResult)
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}