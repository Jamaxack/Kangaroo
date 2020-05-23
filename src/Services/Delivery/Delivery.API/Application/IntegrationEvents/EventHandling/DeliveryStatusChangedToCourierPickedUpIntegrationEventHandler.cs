﻿using Delivery.API.Application.IntegrationEvents.Events;
using Delivery.Domain.AggregatesModel.DeliveryAggregate;
using Delivery.Domain.Exceptions;
using Kangaroo.BuildingBlocks.EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Delivery.API.Application.IntegrationEvents.EventHandling
{
    public class DeliveryStatusChangedToCourierPickedUpIntegrationEventHandler : IIntegrationEventHandler<DeliveryStatusChangedToCourierPickedUpIntegrationEvent>
    {
        readonly IDeliveryRepository _deliveryRepository;
        readonly ILogger<DeliveryStatusChangedToCourierPickedUpIntegrationEventHandler> _logger;

        public DeliveryStatusChangedToCourierPickedUpIntegrationEventHandler(IDeliveryRepository deliveryRepository, ILogger<DeliveryStatusChangedToCourierPickedUpIntegrationEventHandler> logger)
        {
            _deliveryRepository = deliveryRepository;
            _logger = logger;
        }

        public async Task Handle(DeliveryStatusChangedToCourierPickedUpIntegrationEvent @event)
        {
            var delivery = await _deliveryRepository.GetAsync(@event.DeliveryId);
            if (delivery == null)
                throw new DeliveryDomainException($"Delivery not found with specified id: {@event.DeliveryId}");

            _logger.LogInformation("----- Setting Delivery status to CourierPickedUp: {@delivery}", delivery);

            delivery.SetCourierPickedUpDeliveryStatus();

            await _deliveryRepository.UnitOfWork.SaveEntitiesAsync();
        }
    }
}