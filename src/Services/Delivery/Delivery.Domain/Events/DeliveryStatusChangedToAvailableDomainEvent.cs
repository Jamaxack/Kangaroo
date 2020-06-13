using System;
using MediatR;

namespace Delivery.Domain.Events
{
    public class DeliveryStatusChangedToAvailableDomainEvent : INotification
    {
        public DeliveryStatusChangedToAvailableDomainEvent(Guid deliveryId, int deliveryStatusBeforeChange)
        {
            DeliveryId = deliveryId;
            DeliveryStatusBeforeChange = deliveryStatusBeforeChange;
        }

        public Guid DeliveryId { get; }
        public int DeliveryStatusBeforeChange { get; }
    }
}