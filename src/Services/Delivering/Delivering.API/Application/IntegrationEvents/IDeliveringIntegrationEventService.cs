using Kangaroo.BuildingBlocks.EventBus.Events;
using System;
using System.Threading.Tasks;

namespace Delivering.API.Application.IntegrationEvents
{
    public interface IDeliveringIntegrationEventService
    {
        Task PublishEventsThroughEventBusAsync(Guid transactionId);
        Task AddAndSaveEventAsync(IntegrationEvent evt);
    }
}
