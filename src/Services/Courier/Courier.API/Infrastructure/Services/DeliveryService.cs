using AutoMapper;
using Courier.API.DataTransferableObjects;
using Courier.API.Infrastructure.Exceptions;
using Courier.API.Infrastructure.Repositories;
using Courier.API.IntegrationEvents;
using Courier.API.IntegrationEvents.Events;
using Courier.API.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Courier.API.Infrastructure.Services
{
    public class DeliveryService : IDeliveryService
    {
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly ICourierRepository _courierRepository;
        private readonly IMapper _mapper;
        private readonly IIntegrationEventPublisher _integrationEventPublisher;

        public DeliveryService(IDeliveryRepository deliveryRepository, ICourierRepository courierRepository, IMapper mapper, IIntegrationEventPublisher integrationEventPublisher)
        {
            _mapper = mapper;
            _integrationEventPublisher = integrationEventPublisher;
            _deliveryRepository = deliveryRepository;
            _courierRepository = courierRepository;
        }

        public async Task<List<DeliveryDto>> GetAvailableDeliveriesAsync()
        {
            var deliveries = await _deliveryRepository.GetAvailableDeliveriesAsync();
            return _mapper.Map<List<DeliveryDto>>(deliveries);
        }

        public async Task<List<DeliveryDto>> GetDeliveriesByCourierIdAsync(Guid courierId)
        {
            if (courierId == Guid.Empty)
                throw new CourierDomainException("Courier Id is not specified");

            var deliveries = await _deliveryRepository.GetDeliveriesByCourierIdAsync(courierId);
            return _mapper.Map<List<DeliveryDto>>(deliveries);
        }

        public async Task<DeliveryDto> GetDeliveryByIdAsync(Guid deliveryId)
        {
            if (deliveryId == Guid.Empty)
                throw new CourierDomainException("Delivery Id is not specified");

            var delivery = await _deliveryRepository.GetDeliveryByIdAsync(deliveryId);
            if (delivery == null)
                throw new KeyNotFoundException($"Delivery not found with specified Id: {deliveryId}");

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
            if (deliveryId == Guid.Empty)
                throw new CourierDomainException("Delivery Id is not specified");

            return _deliveryRepository.DeleteDeliveryByIdAsync(deliveryId);
        }

        public async Task AssignCourierToDeliveryAsync(AssignCourierToDeliveryDtoSave assignCourierToDelivery)
        {
            var deliveryId = assignCourierToDelivery.DeliveryId;
            var courierId = assignCourierToDelivery.CourierId;

            var delivery = await _deliveryRepository.GetDeliveryByIdAsync(deliveryId);
            if (delivery == null)
                throw new KeyNotFoundException($"Delivery not found with specified Id: {deliveryId}");

            var courier = await _courierRepository.GetCourierByIdAsync(courierId);
            if (courier == null)
                throw new KeyNotFoundException($"Courier not found with specified Id: {courierId}");

            await _deliveryRepository.AssignCourierToDeliveryAsync(deliveryId, courierId);
            await _deliveryRepository.SetDeliveryStatusAsync(deliveryId, DeliveryStatus.CourierAssigned);
            var deliveryStatusChangedEvent = new CourierAssignedToDeliveryIntegrationEvent(deliveryId, courierId);
            _integrationEventPublisher.Publish(deliveryStatusChangedEvent);
        }

        public async Task SetDeliveryStatusToCourierPickedUpAsync(Guid deliveryId)
        {
            if (deliveryId == Guid.Empty)
                throw new CourierDomainException("Delivery Id is not specified");

            var delivery = await _deliveryRepository.GetDeliveryByIdAsync(deliveryId);
            if (delivery == null)
                throw new KeyNotFoundException($"Delivery not found with specified Id: {deliveryId}");

            await _deliveryRepository.SetDeliveryStatusAsync(deliveryId, DeliveryStatus.CourierPickedUp);
            var deliveryStatusChangedEvent = new DeliveryStatusChangedToCourierPickedUpIntegrationEvent(deliveryId);
            _integrationEventPublisher.Publish(deliveryStatusChangedEvent);
        }
    }
}