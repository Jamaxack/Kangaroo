using System;
using MediatR;

namespace Delivery.Domain.Events
{
    public class DeliveryCreatedDomainEvent : INotification
    {
        public DeliveryCreatedDomainEvent(AggregatesModel.DeliveryAggregate.Delivery delivery, Guid clientId)
        {
            ClientId = clientId;
            Delivery = delivery;
        }

        public Guid ClientId { get; }
        public AggregatesModel.DeliveryAggregate.Delivery Delivery { get; }
    }
}