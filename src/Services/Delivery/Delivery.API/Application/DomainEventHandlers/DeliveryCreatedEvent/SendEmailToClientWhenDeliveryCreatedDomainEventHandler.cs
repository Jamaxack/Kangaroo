using System.Threading;
using System.Threading.Tasks;
using Delivery.Domain.Events;
using MediatR;

namespace Delivery.API.Application.DomainEventHandlers.DeliveryCreatedEvent
{
    public class SendEmailToClientWhenDeliveryCreatedDomainEventHandler : INotificationHandler<DeliveryCreatedDomainEvent>
    {
        public Task Handle(DeliveryCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            //TODO: Email service should send an email to Client with Delivery details
            return Task.CompletedTask;
        }
    }
}
