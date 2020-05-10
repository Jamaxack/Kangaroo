using Courier.API.Model;
using Kangaroo.BuildingBlocks.EventBus.Events;
using System;

namespace Courier.API.IntegrationEvents.Events
{
    public class DeliveryStatusChangedToCourierPickedUpIntegrationEvent : IntegrationEvent
    {
        public Guid DeliveryId { get; set; }

        public DeliveryStatusChangedToCourierPickedUpIntegrationEvent(Guid deliveryId)
        {
            DeliveryId = deliveryId;
        }
    }
}
