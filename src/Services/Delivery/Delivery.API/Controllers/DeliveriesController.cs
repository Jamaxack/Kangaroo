using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Delivery.API.Application.Commands;
using Delivery.API.Application.Commands.DeliveryAggregate;
using Delivery.API.Application.Queries;
using Delivery.API.Application.Queries.ViewModels;
using Kangaroo.BuildingBlocks.EventBus.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Delivery.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DeliveriesController : ControllerBase
    {
        private readonly IDeliveryQueries _deliveryQueries;
        private readonly ILogger<DeliveriesController> _logger;
        private readonly IMediator _mediator;

        public DeliveriesController(
            IMediator mediator,
            IDeliveryQueries deliveryQueries,
            ILogger<DeliveriesController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _deliveryQueries = deliveryQueries;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        //GET ~/api/v1/[controller]/3fa85f64-5717-4562-b3fc-2c963f66afa6 
        [Route("{DeliveryId:Guid}")]
        [HttpGet]
        [ProducesResponseType(typeof(DeliveryViewModel), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> ByDeliveryIdAsync(Guid deliveryId)
        {
            try
            {
                var delivery = await _deliveryQueries.GetDeliveryByIdAsync(deliveryId);
                return Ok(delivery);
            }
            catch
            {
                return NotFound();
            }
        }

        //GET ~/api/v1/[controller]/ByClientId/3fa85f64-5717-4562-b3fc-2c963f66afa6 
        [Route("ByClientId/{clientId:Guid}")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DeliveryViewModel>), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> ByClientIdAsync(Guid clientId)
        {
            try
            {
                var deliveries = await _deliveryQueries.GetDeliverysByClientIdAsync(clientId);
                return Ok(deliveries);
            }
            catch
            {
                return NotFound();
            }
        }

        //GET ~/api/v1/[controller]/ByCourierId/3fa85f64-5717-4562-b3fc-2c963f66afa6 
        [Route("ByCourierId/{courierId:Guid}")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DeliveryViewModel>), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> ByCourierIdAsync(Guid courierId)
        {
            try
            {
                var deliveries = await _deliveryQueries.GetDeliverysByCourierIdAsync(courierId);
                return Ok(deliveries);
            }
            catch
            {
                return NotFound();
            }
        }

        //GET ~/api/v1/[controller]/[?pageSize=12&pageIndex=7]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DeliveryViewModel>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetAsync([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            var deliveries = await _deliveryQueries.GetDeliverysAsync(pageSize, pageIndex);
            return Ok(deliveries);
        }

        //POST ~/api/v1/[controller]
        [HttpPost]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateAsync(CreateDeliveryCommand createDeliveryCommand,
            [FromHeader(Name = "x-requestid")] Guid requestId)
        {
            var commandResult = false;
            if (requestId != Guid.Empty)
            {
                var requestCreateDelivery =
                    new IdentifiedCommand<CreateDeliveryCommand, bool>(createDeliveryCommand, requestId);

                _logger.LogInformation(
                    "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                    requestCreateDelivery.GetGenericTypeName(),
                    nameof(requestCreateDelivery.Command.ClientId),
                    requestCreateDelivery.Command.ClientId,
                    requestCreateDelivery);

                commandResult = await _mediator.Send(requestCreateDelivery);
            }

            if (!commandResult) return BadRequest();

            return Ok();
        }

        //POST ~/api/v1/[controller]/3fa85f64-5717-4562-b3fc-2c963f66afa6/SetStatusToAvailable
        [Route("{DeliveryId:Guid}/SetStatusToAvailable")]
        [HttpPost]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SetStatusToAvailableAsync(
            [FromRoute] SetAvailableDeliveryStatusCommand setAvailableDeliveryStatusCommand,
            [FromHeader(Name = "x-requestid")] Guid requestId)
        {
            var commandResult = false;
            if (requestId != Guid.Empty)
            {
                var requestSetAvailableDeliveryStatusCommand =
                    new IdentifiedCommand<SetAvailableDeliveryStatusCommand, bool>(setAvailableDeliveryStatusCommand,
                        requestId);

                _logger.LogInformation(
                    "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                    requestSetAvailableDeliveryStatusCommand.GetGenericTypeName(),
                    nameof(requestSetAvailableDeliveryStatusCommand.Command.DeliveryId),
                    requestSetAvailableDeliveryStatusCommand.Command.DeliveryId,
                    requestSetAvailableDeliveryStatusCommand);

                commandResult = await _mediator.Send(requestSetAvailableDeliveryStatusCommand);
            }

            if (!commandResult) return BadRequest();

            return Ok();
        }

        //POST ~/api/v1/[controller]/3fa85f64-5717-4562-b3fc-2c963f66afa6
        [Route("{DeliveryId:Guid}")]
        [HttpDelete]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteAsync([FromRoute] DeleteDeliveryCommand deleteDeliveryCommand,
            [FromHeader(Name = "x-requestid")] Guid requestId)
        {
            var commandResult = false;
            if (requestId != Guid.Empty)
            {
                var requestDeleteDelivery =
                    new IdentifiedCommand<DeleteDeliveryCommand, bool>(deleteDeliveryCommand, requestId);

                _logger.LogInformation(
                    "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                    requestDeleteDelivery.GetGenericTypeName(),
                    nameof(requestDeleteDelivery.Command.DeliveryId),
                    requestDeleteDelivery.Command.DeliveryId,
                    requestDeleteDelivery);

                commandResult = await _mediator.Send(requestDeleteDelivery);
            }

            if (!commandResult) return BadRequest();

            return NoContent();
        }
    }
}