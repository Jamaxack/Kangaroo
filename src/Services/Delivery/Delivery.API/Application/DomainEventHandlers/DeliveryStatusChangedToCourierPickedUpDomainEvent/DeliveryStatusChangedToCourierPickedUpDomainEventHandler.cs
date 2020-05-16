using Delivery.Domain.AggregatesModel.DeliveryAggregate;
using Delivery.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Delivery.API.Application.DomainEventHandlers
{
    public class DeliveryStatusChangedToCourierPickedUpDomainEventHandler : INotificationHandler<DeliveryStatusChangedToCourierPickedUpDomainEvent>
    {
        private readonly ILogger<DeliveryStatusChangedToCourierPickedUpDomainEventHandler> _logger;

        public DeliveryStatusChangedToCourierPickedUpDomainEventHandler(ILogger<DeliveryStatusChangedToCourierPickedUpDomainEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(DeliveryStatusChangedToCourierPickedUpDomainEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Delivery with Id: {DeliveryId} has been successfully updated status from {DeliveryStatusBeforeChange} to {Status} ({Id})",
                    notification.DeliveryId, DeliveryStatus.From(notification.DeliveryStatusBeforeChange), nameof(DeliveryStatus.CourierPickedUp), DeliveryStatus.CourierPickedUp.Id);

            return Task.CompletedTask;
        }
    }
}
