using MediatR;
using System;

namespace DeliveryOrder.Domain.Events
{
    public class DeliveryOrderCreatedDomainEvent : INotification
    {
        public Guid ClientId { get; }
        public AggregatesModel.DeliveryOrderAggregate.DeliveryOrder DeliveryOrder { get; }

        public DeliveryOrderCreatedDomainEvent(AggregatesModel.DeliveryOrderAggregate.DeliveryOrder deliveryOrder, Guid clientId)
        {
            ClientId = clientId;
            DeliveryOrder = deliveryOrder;
        }
    }
}
