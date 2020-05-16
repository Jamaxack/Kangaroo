namespace Delivery.Domain.Events
{
    using MediatR;
    using System;
    using AggregatesModel.DeliveryAggregate;

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
