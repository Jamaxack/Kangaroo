using Delivering.API.Application.Commands;
using Delivering.API.Application.Queries;
using Kangaroo.BuildingBlocks.EventBus.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Delivering.API.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class DeliveriesController : ControllerBase
    {
        readonly IMediator _mediator;
        readonly ILogger<DeliveriesController> _logger;
        readonly IDeliveryQueries _DeliveryQueries;

        public DeliveriesController(
            IMediator mediator,
            IDeliveryQueries DeliveryQueries,
            ILogger<DeliveriesController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _DeliveryQueries = DeliveryQueries;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        //GET ~/api/v1/[controller]/ByDeliveryId/3fa85f64-5717-4562-b3fc-2c963f66afa6 
        [Route("{DeliveryId:Guid}")]
        [HttpGet]
        [ProducesResponseType(typeof(DeliveryViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> ByDeliveryIdAsync(Guid DeliveryId)
        {
            try
            {
                var Delivery = await _DeliveryQueries.GetDeliveryByIdAsync(DeliveryId);
                return Ok(Delivery);
            }
            catch
            {
                return NotFound();
            }
        }

        //GET ~/api/v1/[controller]/ByClientId/3fa85f64-5717-4562-b3fc-2c963f66afa6 
        [Route("{clientId:Guid}")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DeliveryViewModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> ByClientIdAsync(Guid clientId)
        {
            try
            {
                var deliveries = await _DeliveryQueries.GetDeliverysByClientIdAsync(clientId);
                return Ok(deliveries);
            }
            catch
            {
                return NotFound();
            }
        }

        //GET ~/api/v1/[controller]/ByCourierId/3fa85f64-5717-4562-b3fc-2c963f66afa6 
        [Route("{courierId:Guid}")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DeliveryViewModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> ByCourierIdAsync(Guid courierId)
        {
            try
            {
                var deliveries = await _DeliveryQueries.GetDeliverysByCourierIdAsync(courierId);
                return Ok(deliveries);
            }
            catch
            {
                return NotFound();
            }
        }

        //GET ~/api/v1/[controller]/Get[?pageSize=12&pageIndex=7]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DeliveryViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAsync([FromQuery]int pageSize = 10, [FromQuery]int pageIndex = 0)
        {
            var deliveries = await _DeliveryQueries.GetDeliverysAsync(pageSize, pageIndex);
            return Ok(deliveries);
        }

        //POST ~/api/v1/[controller]/Create
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateAsync(CreateDeliveryCommand createDeliveryCommand, [FromHeader(Name = "x-requestid")] Guid requestId)
        {
            bool commandResult = false;
            if (requestId != Guid.Empty)
            {
                var requestCreateDelivery = new IdentifiedCommand<CreateDeliveryCommand, bool>(createDeliveryCommand, requestId);

                _logger.LogInformation(
                    "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                    requestCreateDelivery.GetGenericTypeName(),
                    nameof(requestCreateDelivery.Command.ClientId),
                    requestCreateDelivery.Command.ClientId,
                    requestCreateDelivery);

                commandResult = await _mediator.Send(requestCreateDelivery);
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
        public async Task<IActionResult> SetStatusToAvailableAsync(SetAvailableDeliveryStatusCommand setAvailableDeliveryStatusCommand, [FromHeader(Name = "x-requestid")] Guid requestId)
        {
            bool commandResult = false;
            if (requestId != Guid.Empty)
            {
                var requestSetAvailableDeliveryStatusCommand = new IdentifiedCommand<SetAvailableDeliveryStatusCommand, bool>(setAvailableDeliveryStatusCommand, requestId);

                _logger.LogInformation(
                    "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                    requestSetAvailableDeliveryStatusCommand.GetGenericTypeName(),
                    nameof(requestSetAvailableDeliveryStatusCommand.Command.DeliveryId),
                    requestSetAvailableDeliveryStatusCommand.Command.DeliveryId,
                    requestSetAvailableDeliveryStatusCommand);

                commandResult = await _mediator.Send(requestSetAvailableDeliveryStatusCommand);
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
        public async Task<IActionResult> DeleteAsync(DeleteDeliveryCommand deleteDeliveryCommand, [FromHeader(Name = "x-requestid")] Guid requestId)
        {
            bool commandResult = false;
            if (requestId != Guid.Empty)
            {
                var requestDeleteDelivery = new IdentifiedCommand<DeleteDeliveryCommand, bool>(deleteDeliveryCommand, requestId);

                _logger.LogInformation(
                    "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                    requestDeleteDelivery.GetGenericTypeName(),
                    nameof(requestDeleteDelivery.Command.DeliveryId),
                    requestDeleteDelivery.Command.DeliveryId,
                    requestDeleteDelivery);

                commandResult = await _mediator.Send(requestDeleteDelivery);
            }

            if (!commandResult)
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}