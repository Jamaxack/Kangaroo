using Delivery.API.Application.IntegrationEvents.Events;
using Delivery.Domain.AggregatesModel.DeliveryAggregate;
using Delivery.Domain.Exceptions;
using Kangaroo.BuildingBlocks.EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Delivery.API.Application.IntegrationEvents.EventHandling
{
    public class CourierAssignedToDeliveryIntegrationEventHandler : IIntegrationEventHandler<CourierAssignedToDeliveryIntegrationEvent>
    {
        readonly IDeliveryRepository _deliveryRepository;
        readonly ILogger<CourierAssignedToDeliveryIntegrationEventHandler> _logger;

        public CourierAssignedToDeliveryIntegrationEventHandler(IDeliveryRepository deliveryRepository, ILogger<CourierAssignedToDeliveryIntegrationEventHandler> logger)
        {
            _deliveryRepository = deliveryRepository;
            _logger = logger;
        }

        public async Task Handle(CourierAssignedToDeliveryIntegrationEvent @event)
        {
            var delivery = await _deliveryRepository.GetAsync(@event.DeliveryId);
            if (delivery == null)
                throw new DeliveryDomainException($"Delivery not found with specified id: {@event.DeliveryId}");

            delivery.AssignCourier(@event.CourierId);

            _logger.LogInformation("----- Courier assigned to delivery: {@delivery}", delivery);

            await _deliveryRepository.UnitOfWork.SaveEntitiesAsync();
        }
    }
}
