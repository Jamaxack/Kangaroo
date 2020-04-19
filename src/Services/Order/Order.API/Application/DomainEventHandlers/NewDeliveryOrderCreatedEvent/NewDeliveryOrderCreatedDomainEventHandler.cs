using MediatR;
using Order.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Order.API.Application.DomainEventHandlers.NewDeliveryOrderCreatedEvent
{
    public class NewDeliveryOrderCreatedDomainEventHandler : INotificationHandler<NewDeliveryOrderCreatedDomainEvent>
    {
        public Task Handle(NewDeliveryOrderCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
