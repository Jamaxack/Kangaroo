using MediatR;
using System;

namespace Delivery.Domain.Events
{
    public class DeliveryStatusChangedToCourierPickedUpDomainEvent : INotification
    {
        public Guid DeliveryId { get; }
        public int DeliveryStatusBeforeChange { get; }

        public DeliveryStatusChangedToCourierPickedUpDomainEvent(Guid deliveryId, int deliveryStatusBeforeChange)
        {
            DeliveryId = deliveryId;
            DeliveryStatusBeforeChange = deliveryStatusBeforeChange;
        }
    }
}
