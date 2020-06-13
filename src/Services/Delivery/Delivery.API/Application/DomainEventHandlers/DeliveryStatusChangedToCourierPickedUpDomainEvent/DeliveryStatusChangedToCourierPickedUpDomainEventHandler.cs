using System.Threading;
using System.Threading.Tasks;
using Delivery.Domain.AggregatesModel.DeliveryAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Delivery.API.Application.DomainEventHandlers.DeliveryStatusChangedToCourierPickedUpDomainEvent
{
    public class DeliveryStatusChangedToCourierPickedUpDomainEventHandler : INotificationHandler<
        Domain.Events.DeliveryStatusChangedToCourierPickedUpDomainEvent>
    {
        private readonly ILogger<DeliveryStatusChangedToCourierPickedUpDomainEventHandler> _logger;

        public DeliveryStatusChangedToCourierPickedUpDomainEventHandler(
            ILogger<DeliveryStatusChangedToCourierPickedUpDomainEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(Domain.Events.DeliveryStatusChangedToCourierPickedUpDomainEvent notification,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Delivery with Id: {DeliveryId} has been successfully updated status from {DeliveryStatusBeforeChange} to {Status} ({Id})",
                notification.DeliveryId, DeliveryStatus.From(notification.DeliveryStatusBeforeChange),
                nameof(DeliveryStatus.CourierPickedUp), DeliveryStatus.CourierPickedUp.Id);

            return Task.CompletedTask;
        }
    }
}