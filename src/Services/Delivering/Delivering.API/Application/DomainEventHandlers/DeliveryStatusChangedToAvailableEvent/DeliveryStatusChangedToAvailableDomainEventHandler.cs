using MediatR;
using Microsoft.Extensions.Logging;
using Delivering.Domain.AggregatesModel.DeliveryAggregate;
using Delivering.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Delivering.API.Application.DomainEventHandlers.DeliveryStatusChangedToAvailableEvent
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

            //TODO: Publish Integration event

            return Task.CompletedTask;
        }
    }
}
