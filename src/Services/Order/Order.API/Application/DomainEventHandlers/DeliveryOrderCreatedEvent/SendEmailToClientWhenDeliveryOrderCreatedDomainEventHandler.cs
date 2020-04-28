﻿using MediatR;
using Order.Domain.Events;
using System.Threading;
using System.Threading.Tasks;

namespace Order.API.Application.DomainEventHandlers.NewDeliveryOrderCreatedEvent
{
    public class SendEmailToClientWhenDeliveryOrderCreatedDomainEventHandler : INotificationHandler<DeliveryOrderCreatedDomainEvent>
    {
        public Task Handle(DeliveryOrderCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            //TODO: Email service should send an email to Client with DeliveryOrder details
            return Task.CompletedTask;
        }
    }
}
