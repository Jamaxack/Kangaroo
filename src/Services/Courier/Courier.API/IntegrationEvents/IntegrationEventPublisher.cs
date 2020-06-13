using Kangaroo.BuildingBlocks.EventBus.Abstractions;
using Kangaroo.BuildingBlocks.EventBus.Events;
using Microsoft.Extensions.Logging;

namespace Courier.API.IntegrationEvents
{
    public class IntegrationEventPublisher : IIntegrationEventPublisher
    {
        private readonly IEventBus _eventBus;
        private readonly ILogger<IntegrationEventPublisher> _logger;

        public IntegrationEventPublisher(IEventBus eventBus,
            ILogger<IntegrationEventPublisher> logger)
        {
            _eventBus = eventBus;
            _logger = logger;
        }

        public void Publish(IntegrationEvent integrationEvent)
        {
            _logger.LogInformation("----- Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})",
              integrationEvent.Id, Program.AppName, integrationEvent);
            _eventBus.Publish(integrationEvent);
        }
    }
}
