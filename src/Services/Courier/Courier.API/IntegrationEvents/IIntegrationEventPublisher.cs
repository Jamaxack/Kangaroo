using Kangaroo.BuildingBlocks.EventBus.Events;

namespace Courier.API.IntegrationEvents
{
    public interface IIntegrationEventPublisher
    {
        void Publish(IntegrationEvent integrationEvent);
    }
}
