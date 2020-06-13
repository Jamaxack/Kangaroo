using System;
using Kangaroo.BuildingBlocks.EventBus.Events;

namespace Courier.API.IntegrationEvents.Events
{
    public class DeliveryStatusChangedToCourierPickedUpIntegrationEvent : IntegrationEvent
    {
        public DeliveryStatusChangedToCourierPickedUpIntegrationEvent(Guid deliveryId)
        {
            DeliveryId = deliveryId;
        }

        public Guid DeliveryId { get; set; }
    }
}