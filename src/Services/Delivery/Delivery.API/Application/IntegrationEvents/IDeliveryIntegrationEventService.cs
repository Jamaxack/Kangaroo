using Kangaroo.BuildingBlocks.EventBus.Events;
using System;
using System.Threading.Tasks;

namespace Delivery.API.Application.IntegrationEvents
{
    public interface IDeliveryIntegrationEventService
    {
        Task PublishEventsThroughEventBusAsync(Guid transactionId);
        Task AddAndSaveEventAsync(IntegrationEvent evt);
    }
}
