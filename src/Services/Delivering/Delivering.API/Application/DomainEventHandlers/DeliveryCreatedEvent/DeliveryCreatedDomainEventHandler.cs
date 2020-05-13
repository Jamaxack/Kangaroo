using Delivering.API.Application.IntegrationEvents;
using Delivering.API.Application.IntegrationEvents.Events;
using Delivering.Domain.AggregatesModel.DeliveryAggregate;
using Delivering.Domain.Events;
using Kangaroo.BuildingBlocks.EventBus.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Delivering.API.Application.DomainEventHandlers.DeliveryCreatedEvent
{
    public class DeliveryCreatedDomainEventHandler : INotificationHandler<DeliveryCreatedDomainEvent>
    {
        readonly IEventBus _eventBus;
        readonly IDeliveringIntegrationEventService _deliveringIntegrationEventService;
        readonly ILogger<DeliveryCreatedDomainEventHandler> _logger;

        public DeliveryCreatedDomainEventHandler(IEventBus eventBus, IDeliveringIntegrationEventService deliveringIntegrationEventService, ILogger<DeliveryCreatedDomainEventHandler> logger)
        {
            _eventBus = eventBus;
            _deliveringIntegrationEventService = deliveringIntegrationEventService;
            _logger = logger;
        }

        public async Task Handle(DeliveryCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var deliveryCreatedIntegrationEvent = MapToDeliveryCreatedIntegrationEvent(notification);
            await _deliveringIntegrationEventService.AddAndSaveEventAsync(deliveryCreatedIntegrationEvent);
        }

        DeliveryCreatedIntegrationEvent MapToDeliveryCreatedIntegrationEvent(DeliveryCreatedDomainEvent notification)
        {
            return new DeliveryCreatedIntegrationEvent()
            {
                ClientId = notification.ClientId,
                CreatedDateTime = notification.Delivery.CreatedDateTime,
                DeliveryId = notification.Delivery.Id,
                Number = notification.Delivery.Number,
                Note = notification.Delivery.Note,
                Price = notification.Delivery.Price,
                Weight = notification.Delivery.Weight,
                PickUpLocation = MapDeliveryLocation(notification.Delivery.PickUpLocation),
                DropOffLocation = MapDeliveryLocation(notification.Delivery.DropOffLocation)
            };
        }

        Models.DeliveryLocation MapDeliveryLocation(DeliveryLocation deliveryLocation)
        {
            return new Models.DeliveryLocation()
            {
                Address = deliveryLocation.Address,
                Note = deliveryLocation.Note,
                ApartmentNumber = deliveryLocation.ApartmentNumber,
                ArrivalFinishDateTime = deliveryLocation.ArrivalFinishDateTime,
                ArrivalStartDateTime = deliveryLocation.ArrivalStartDateTime,
                BuildingNumber = deliveryLocation.BuildingNumber,
                EntranceNumber = deliveryLocation.EntranceNumber,
                FloorNumber = deliveryLocation.FloorNumber,
                Latitude = deliveryLocation.Latitude,
                Longitude = deliveryLocation.Longitude,
                ContactPerson = new Models.ContactPerson()
                {
                    Name = deliveryLocation.ContactPerson.Name,
                    Phone = deliveryLocation.ContactPerson.Phone
                }
            };
        }
    }
}
