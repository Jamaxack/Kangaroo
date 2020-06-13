using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Courier.API.DataTransferableObjects;
using Courier.API.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Courier.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DeliveriesController : ControllerBase
    {
        private readonly IDeliveryService _deliveryService;

        public DeliveriesController(IDeliveryService deliveryService)
        {
            _deliveryService = deliveryService;
        }

        [Route("Available")]
        [HttpGet]
        [ProducesResponseType(typeof(List<DeliveryDto>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetAvailableDeliveriesAsync()
        {
            return Ok(await _deliveryService.GetAvailableDeliveriesAsync());
        }
         
        [Route("ByCourierId/{courierId:Guid}")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(List<DeliveryDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDeliveriesByCourierIdAsync(Guid courierId)
        {
            return Ok(await _deliveryService.GetDeliveriesByCourierIdAsync(courierId));
        }

        [HttpPost]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.Accepted)]
        public async Task<IActionResult> CreateDeliveryAsync(DeliveryDtoSave delivery)
        {
            await _deliveryService.InsertDeliveryAsync(delivery);
            return Accepted();
        }

        [Route("AssignCourier")]
        [HttpPost]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.Accepted)]
        public async Task<IActionResult> AssignCourierToDeliveryAsync(
            AssignCourierToDeliveryDtoSave assignCourierToDelivery)
        {
            await _deliveryService.AssignCourierToDeliveryAsync(assignCourierToDelivery);
            return Accepted();
        }

        [Route("{deliveryId}")]
        [HttpGet]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(DeliveryDto), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetDeliveryByIdAsync(Guid deliveryId)
        {
            return Ok(await _deliveryService.GetDeliveryByIdAsync(deliveryId));
        }

        [Route("{deliveryId}")]
        [HttpDelete]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.Accepted)]
        public async Task<IActionResult> DeleteDeliveryByIdAsync(Guid deliveryId)
        {
            await _deliveryService.DeleteDeliveryByIdAsync(deliveryId);
            return Accepted();
        }

        [Route("{deliveryId}/CourierPickedUp")]
        [HttpPost]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.Accepted)]
        public async Task<IActionResult> PickedUpAsync(Guid deliveryId)
        {
            await _deliveryService.SetDeliveryStatusToCourierPickedUpAsync(deliveryId);
            return Accepted();
        }
    }
}