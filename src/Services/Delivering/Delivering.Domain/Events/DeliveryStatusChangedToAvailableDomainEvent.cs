using MediatR;
using Delivering.Domain.AggregatesModel.DeliveryAggregate;
using System;
using System.Collections.Generic;

namespace Delivering.Domain.Events
{
    public class DeliveryStatusChangedToAvailableDomainEvent : INotification
    {
        public Guid DeliveryId { get; }
        public int DeliveryStatusBeforeChange { get; }

        public DeliveryStatusChangedToAvailableDomainEvent(Guid DeliveryId, int DeliveryStatusBeforeChange)
        {
            DeliveryId = DeliveryId;
            DeliveryStatusBeforeChange = DeliveryStatusBeforeChange;
        }
    }
}
