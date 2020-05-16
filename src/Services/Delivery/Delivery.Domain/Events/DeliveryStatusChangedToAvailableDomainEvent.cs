using MediatR;
using System;

namespace Delivery.Domain.Events
{
    public class DeliveryStatusChangedToAvailableDomainEvent : INotification
    {
        public Guid DeliveryId { get; }
        public int DeliveryStatusBeforeChange { get; }

        public DeliveryStatusChangedToAvailableDomainEvent(Guid deliveryId, int deliveryStatusBeforeChange)
        {
            DeliveryId = deliveryId;
            DeliveryStatusBeforeChange = deliveryStatusBeforeChange;
        }
    }
}
