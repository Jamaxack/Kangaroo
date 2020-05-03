using MediatR;
using Microsoft.Extensions.Logging;
using Delivery.Domain.AggregatesModel.DeliveryOrderAggregate;
using Delivery.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Delivery.API.Application.DomainEventHandlers.DeliveryOrderStatusChangedToAvailableEvent
{
    public class DeliveryOrderStatusChangedToAvailableDomainEventHandler : INotificationHandler<DeliveryOrderStatusChangedToAvailableDomainEvent>
    {
        private readonly ILogger<DeliveryOrderStatusChangedToAvailableDomainEventHandler> _logger;

        public DeliveryOrderStatusChangedToAvailableDomainEventHandler(ILogger<DeliveryOrderStatusChangedToAvailableDomainEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(DeliveryOrderStatusChangedToAvailableDomainEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("DeliveryOrder with Id: {DeliveryOrderId} has been successfully updated status from {DeliveryOrderStatusBeforeChange} to {Status} ({Id})",
                    notification.DeliveryOrderId, DeliveryOrderStatus.From(notification.DeliveryOrderStatusBeforeChange), nameof(DeliveryOrderStatus.Available), DeliveryOrderStatus.Available.Id);

            //TODO: Publish Integration event

            return Task.CompletedTask;
        }
    }
}
