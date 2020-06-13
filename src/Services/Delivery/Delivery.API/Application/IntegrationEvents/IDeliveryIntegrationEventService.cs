using System;
using System.Threading.Tasks;
using Kangaroo.BuildingBlocks.EventBus.Events;

namespace Delivery.API.Application.IntegrationEvents
{
    public interface IDeliveryIntegrationEventService
    {
        Task PublishEventsThroughEventBusAsync(Guid transactionId);
        Task AddAndSaveEventAsync(IntegrationEvent evt);
    }
}