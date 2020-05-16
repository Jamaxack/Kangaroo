using Kangaroo.BuildingBlocks.EventBus.Events;
using System;

namespace Delivery.API.Application.IntegrationEvents.Events
{
    public class CourierAssignedToDeliveryIntegrationEvent : IntegrationEvent
    {
        public Guid DeliveryId { get; set; }
        public Guid CourierId { get; set; }

        public CourierAssignedToDeliveryIntegrationEvent(Guid deliveryId, Guid courierId)
        {
            DeliveryId = deliveryId;
            CourierId = courierId;
        }
    }
}
