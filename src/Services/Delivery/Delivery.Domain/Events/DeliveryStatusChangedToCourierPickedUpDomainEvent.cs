using System;
using MediatR;

namespace Delivery.Domain.Events
{
    public class DeliveryStatusChangedToCourierPickedUpDomainEvent : INotification
    {
        public DeliveryStatusChangedToCourierPickedUpDomainEvent(Guid deliveryId, int deliveryStatusBeforeChange)
        {
            DeliveryId = deliveryId;
            DeliveryStatusBeforeChange = deliveryStatusBeforeChange;
        }

        public Guid DeliveryId { get; }
        public int DeliveryStatusBeforeChange { get; }
    }
}