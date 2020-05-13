using AutoMapper;
using Courier.API.Infrastructure.Services;
using Courier.API.IntegrationEvents.Events;
using Courier.API.Model;
using Kangaroo.BuildingBlocks.EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Courier.API.IntegrationEvents.EventHandling
{
    public class DeliveryCreatedIntegrationEventHandler : IIntegrationEventHandler<DeliveryCreatedIntegrationEvent>
    {
        private readonly IDeliveryService _deliveryService;
        private readonly IMapper _mapper;
        private readonly ILogger<DeliveryCreatedIntegrationEventHandler> _logger;

        public DeliveryCreatedIntegrationEventHandler(IDeliveryService deliveryService, IMapper mapper, ILogger<DeliveryCreatedIntegrationEventHandler> logger)
        {
            _deliveryService = deliveryService;
            _mapper = mapper;
            _logger = logger;
        }

        public Task Handle(DeliveryCreatedIntegrationEvent @event)
        {
            _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);
            var delivery = _mapper.Map<Delivery>(@event);
            return _deliveryService.InsertDeliveryAsync(delivery);
        }
    }
}
