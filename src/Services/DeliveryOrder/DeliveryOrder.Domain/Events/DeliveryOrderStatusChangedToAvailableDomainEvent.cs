using MediatR;
using DeliveryOrder.Domain.AggregatesModel.DeliveryOrderAggregate;
using System;
using System.Collections.Generic;

namespace DeliveryOrder.Domain.Events
{
    public class DeliveryOrderStatusChangedToAvailableDomainEvent : INotification
    {
        public Guid DeliveryOrderId { get; }
        public int DeliveryOrderStatusBeforeChange { get; }

        public DeliveryOrderStatusChangedToAvailableDomainEvent(Guid deliveryOrderId, int deliveryOrderStatusBeforeChange)
        {
            DeliveryOrderId = deliveryOrderId;
            DeliveryOrderStatusBeforeChange = deliveryOrderStatusBeforeChange;
        }
    }
}
