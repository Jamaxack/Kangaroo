using MediatR;
using Delivering.Domain.Events;
using System.Threading;
using System.Threading.Tasks;

namespace Delivering.API.Application.DomainEventHandlers
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
