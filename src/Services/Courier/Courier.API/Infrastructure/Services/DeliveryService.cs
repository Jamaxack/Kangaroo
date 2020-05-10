using Courier.API.Infrastructure.Exceptions;
using Courier.API.Infrastructure.Repositories;
using Courier.API.IntegrationEvents.Events;
using Courier.API.Model;
using Kangaroo.BuildingBlocks.EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Courier.API.Infrastructure.Services
{
    public class DeliveryService : IDeliveryService
    {
        readonly IDeliveryRepository _deliveryRepository;
        readonly IEventBus _eventBus;
        readonly ILogger<DeliveryService> _logger;

        public DeliveryService(IDeliveryRepository deliveryRepository, IEventBus eventBus, ILogger<DeliveryService> logger)
        {
            _deliveryRepository = deliveryRepository;
            _eventBus = eventBus;
            _logger = logger;
        }

        public Task<Delivery> GetDeliveryByIdAsync(Guid deliveryId)
        {
            if (deliveryId == Guid.Empty)
                throw new CourierDomainException("Delivery Id is not specified");

            var delivery = _deliveryRepository.GetDeliveryByIdAsync(deliveryId);
            if (delivery == null)
                throw new KeyNotFoundException();
            return delivery;
        }

        public Task InsertDeliveryAsync(Delivery delivery)
        {
            if (delivery == null)
                throw new CourierDomainException("Delivery is null");

            return _deliveryRepository.InsertDeliveryAsync(delivery);
        }

        public Task DeleteDeliveryByIdAsync(Guid deliveryId)
        {
            if (deliveryId == null)
                throw new CourierDomainException("Delivery Id is not specified");

            return _deliveryRepository.DeleteDeliveryByIdAsync(deliveryId);
        }

        public async Task DeliveryStatusChangedToCourierPickedUp(Guid deliveryId)
        {
            if (deliveryId == null)
                throw new CourierDomainException("Delivery Id is not specified");

            await _deliveryRepository.SetDeliveryStatusToCourierPickedUpAsync(deliveryId);

            PublishDeliveryStatusChangedToCourierPickedUpIntegrationEvent(deliveryId);
        }

        void PublishDeliveryStatusChangedToCourierPickedUpIntegrationEvent(Guid deliveryId)
        {
            var deliveryStatusChangedEvent = new DeliveryStatusChangedToCourierPickedUpIntegrationEvent(deliveryId);
            _logger.LogInformation("----- Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", deliveryStatusChangedEvent.Id, Program.AppName, deliveryStatusChangedEvent);
            _eventBus.Publish(deliveryStatusChangedEvent);
        }
    }
}
