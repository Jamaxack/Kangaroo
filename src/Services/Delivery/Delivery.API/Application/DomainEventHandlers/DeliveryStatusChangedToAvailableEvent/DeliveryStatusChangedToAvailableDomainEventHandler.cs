using Delivery.Domain.AggregatesModel.DeliveryAggregate;
using Delivery.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Delivery.API.Application.DomainEventHandlers
{
    public class DeliveryStatusChangedToAvailableDomainEventHandler : INotificationHandler<DeliveryStatusChangedToAvailableDomainEvent>
    {
        private readonly ILogger<DeliveryStatusChangedToAvailableDomainEventHandler> _logger;

        public DeliveryStatusChangedToAvailableDomainEventHandler(ILogger<DeliveryStatusChangedToAvailableDomainEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(DeliveryStatusChangedToAvailableDomainEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Delivery with Id: {DeliveryId} has been successfully updated status from {DeliveryStatusBeforeChange} to {Status} ({Id})",
                    notification.DeliveryId, DeliveryStatus.From(notification.DeliveryStatusBeforeChange), nameof(DeliveryStatus.Available), DeliveryStatus.Available.Id);

            return Task.CompletedTask;
        }
    }
}
