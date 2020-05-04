using MediatR;
using System;

namespace Delivering.Domain.Events
{
    public class DeliveryCreatedDomainEvent : INotification
    {
        public Guid ClientId { get; }
        public AggregatesModel.DeliveryAggregate.Delivery Delivery { get; }

        public DeliveryCreatedDomainEvent(AggregatesModel.DeliveryAggregate.Delivery Delivery, Guid clientId)
        {
            ClientId = clientId;
            Delivery = Delivery;
        }
    }
}
