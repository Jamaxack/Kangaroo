﻿using Courier.API.DataTransferableObjects;
using Courier.API.Infrastructure.Services;
using Courier.API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Courier.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DeliveriesController : ControllerBase
    {
        readonly IDeliveryService _deliveryService;
        readonly ILogger<DeliveriesController> _logger;

        public DeliveriesController(IDeliveryService deliveryService, ILogger<DeliveriesController> logger)
        {
            _deliveryService = deliveryService;
            _logger = logger;
        }

        [Route("Available")]
        [HttpGet]
        [ProducesResponseType(typeof(List<Delivery>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAvailableDeliveriesAsync()
            => Ok(await _deliveryService.GetAvailableDeliveriesAsync());

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        public async Task<IActionResult> CreateDeliveryAsync(Delivery delivery)
        {
            await _deliveryService.InsertDeliveryAsync(delivery);
            return Accepted();
        }

        [Route("AssignCourier")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        public async Task<IActionResult> AssignCourierToDeliveryAsync(AssignCourierToDeliveryDTO assignCourierToDelivery)
        {
            await _deliveryService.AssignCourierToDeliveryAsync(assignCourierToDelivery);
            return Accepted();
        }

        [Route("{deliveryId}")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Delivery), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDeliveryByIdAsync(Guid deliveryId)
            => Ok(await _deliveryService.GetDeliveryByIdAsync(deliveryId));

        [Route("{deliveryId}")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        public async Task<IActionResult> DeleteDeliveryByIdAsync(Guid deliveryId)
        {
            await _deliveryService.DeleteDeliveryByIdAsync(deliveryId);
            return Accepted();
        }

        [Route("{deliveryId}/CourierPickedUp")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        public async Task<IActionResult> PickedUpAsync(Guid deliveryId)
        {
            await _deliveryService.DeliveryStatusChangedToCourierPickedUpAsync(deliveryId);
            return Accepted();
        }
    }
}
