using MediatR;
using Order.Domain.AggregatesModel.DeliveryOrderAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Domain.Events
{
    public class DeliveryOrderCreatedDomainEvent : INotification
    {
        public Guid ClientId { get; }
        public DeliveryOrder DeliveryOrder { get; }

        public DeliveryOrderCreatedDomainEvent(DeliveryOrder deliveryOrder, Guid clientId)
        {
            ClientId = clientId;
            DeliveryOrder = deliveryOrder;
        }
    }
}
