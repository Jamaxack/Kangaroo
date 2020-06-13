using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Courier.API.DataTransferableObjects;
using Courier.API.Infrastructure.Exceptions;
using Courier.API.Infrastructure.Repositories;
using Courier.API.IntegrationEvents.Events;
using Courier.API.Model;
using Kangaroo.BuildingBlocks.EventBus.Abstractions;
using Kangaroo.BuildingBlocks.EventBus.Events;
using Microsoft.Extensions.Logging;

namespace Courier.API.Infrastructure.Services
{
    public class DeliveryService : IDeliveryService
    {
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly IEventBus _eventBus;
        private readonly ILogger<DeliveryService> _logger;
        private readonly IMapper _mapper;

        public DeliveryService(IDeliveryRepository deliveryRepository, IEventBus eventBus, IMapper mapper,
            ILogger<DeliveryService> logger)
        {
            _mapper = mapper;
            _deliveryRepository = deliveryRepository;
            _eventBus = eventBus;
            _logger = logger;
        }

        public async Task<List<DeliveryDto>> GetAvailableDeliveriesAsync()
        {
            var deliveries = await _deliveryRepository.GetAvailableDeliveriesAsync();
            return _mapper.Map<List<DeliveryDto>>(deliveries);
        }

        public async Task AssignCourierToDeliveryAsync(AssignCourierToDeliveryDtoSave assignCourierToDelivery)
        {
            var deliveryId = assignCourierToDelivery.DeliveryId;
            var courierId = assignCourierToDelivery.CourierId;

            var delivery = await _deliveryRepository.GetDeliveryByIdAsync(deliveryId);
            if (delivery == null)
                throw new KeyNotFoundException();

            await _deliveryRepository.AssignCourierToDeliveryAsync(deliveryId, courierId);
            await DeliveryStatusChangedToCourierAssignedAsync(deliveryId, courierId);
        }

        public async Task<DeliveryDto> GetDeliveryByIdAsync(Guid deliveryId)
        {
            if (deliveryId == Guid.Empty)
                throw new CourierDomainException("Delivery Id is not specified");

            var delivery = await _deliveryRepository.GetDeliveryByIdAsync(deliveryId);
            if (delivery == null)
                throw new KeyNotFoundException();
            return _mapper.Map<DeliveryDto>(delivery);
        }

        public Task InsertDeliveryAsync(DeliveryDtoSave deliveryDtoSave)
        {
            if (deliveryDtoSave == null)
                throw new CourierDomainException("Delivery is null");

            var delivery = _mapper.Map<Delivery>(deliveryDtoSave);
            return _deliveryRepository.InsertDeliveryAsync(delivery);
        }

        public Task DeleteDeliveryByIdAsync(Guid deliveryId)
        {
            if (deliveryId == null)
                throw new CourierDomainException("Delivery Id is not specified");

            return _deliveryRepository.DeleteDeliveryByIdAsync(deliveryId);
        }

        public async Task DeliveryStatusChangedToCourierAssignedAsync(Guid deliveryId, Guid courierId)
        {
            if (deliveryId == null)
                throw new CourierDomainException("Delivery Id is not specified");

            await _deliveryRepository.SetDeliveryStatusAsync(deliveryId, DeliveryStatus.CourierAssigned);

            var deliveryStatusChangedEvent = new CourierAssignedToDeliveryIntegrationEvent(deliveryId, courierId);
            PublishIntegrationEvent(deliveryStatusChangedEvent);
        }

        public async Task DeliveryStatusChangedToCourierPickedUpAsync(Guid deliveryId)
        {
            if (deliveryId == null)
                throw new CourierDomainException("Delivery Id is not specified");

            await _deliveryRepository.SetDeliveryStatusAsync(deliveryId, DeliveryStatus.CourierPickedUp);
            var deliveryStatusChangedEvent = new DeliveryStatusChangedToCourierPickedUpIntegrationEvent(deliveryId);
            PublishIntegrationEvent(deliveryStatusChangedEvent);
        }

        private void PublishIntegrationEvent(IntegrationEvent integrationEvent)
        {
            _logger.LogInformation(
                "----- Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})",
                integrationEvent.Id, Program.AppName, integrationEvent);
            _eventBus.Publish(integrationEvent);
        }
    }
}