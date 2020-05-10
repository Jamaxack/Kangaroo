using Kangaroo.BuildingBlocks.EventBus.Events;
using System;

namespace Delivering.API.Application.IntegrationEvents.Events
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
