using System.Threading;
using System.Threading.Tasks;
using Delivery.API.Application.IntegrationEvents;
using Delivery.API.Application.IntegrationEvents.Events;
using Delivery.API.Application.Models;
using Delivery.Domain.Events;
using MediatR;

namespace Delivery.API.Application.DomainEventHandlers.DeliveryCreatedEvent
{
    public class DeliveryCreatedDomainEventHandler : INotificationHandler<DeliveryCreatedDomainEvent>
    {
        private readonly IDeliveryIntegrationEventService _deliveryIntegrationEventService; 

        public DeliveryCreatedDomainEventHandler(IDeliveryIntegrationEventService deliveryIntegrationEventService)
        { 
            _deliveryIntegrationEventService = deliveryIntegrationEventService; 
        }

        public async Task Handle(DeliveryCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var deliveryCreatedIntegrationEvent = MapToDeliveryCreatedIntegrationEvent(notification);
            await _deliveryIntegrationEventService.AddAndSaveEventAsync(deliveryCreatedIntegrationEvent);
        }

        private DeliveryCreatedIntegrationEvent MapToDeliveryCreatedIntegrationEvent(
            DeliveryCreatedDomainEvent notification)
        {
            return new DeliveryCreatedIntegrationEvent
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

        private DeliveryLocation MapDeliveryLocation(
            Domain.AggregatesModel.DeliveryAggregate.DeliveryLocation deliveryLocation)
        {
            return new DeliveryLocation
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
                ContactPerson = new ContactPerson
                {
                    Name = deliveryLocation.ContactPerson.Name,
                    Phone = deliveryLocation.ContactPerson.Phone
                }
            };
        }
    }
}