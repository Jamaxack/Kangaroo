namespace Delivery.Domain.Events
{
    using AggregatesModel.DeliveryAggregate;
    using MediatR;
    using System;

    public class DeliveryCreatedDomainEvent : INotification
    {
        public Guid ClientId { get; }
        public Delivery Delivery { get; }

        public DeliveryCreatedDomainEvent(Delivery delivery, Guid clientId)
        {
            ClientId = clientId;
            Delivery = delivery;
        }
    }
}
