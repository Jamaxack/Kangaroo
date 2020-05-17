using AutoMapper;
using Courier.API.Infrastructure.GrpcServices;
using Courier.API.Infrastructure.Repositories;
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
        readonly IDeliveryService _deliveryService;
        readonly IClientGrpcService _clientGrpcService;
        readonly IClientRepository _clientRepository;
        readonly IMapper _mapper;
        readonly ILogger<DeliveryCreatedIntegrationEventHandler> _logger;

        public DeliveryCreatedIntegrationEventHandler(IDeliveryService deliveryService, IClientGrpcService clientGrpcService, IClientRepository clientRepository, IMapper mapper, ILogger<DeliveryCreatedIntegrationEventHandler> logger)
        {
            _deliveryService = deliveryService;
            _clientGrpcService = clientGrpcService;
            _clientRepository = clientRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Handle(DeliveryCreatedIntegrationEvent @event)
        {
            _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);
            var delivery = _mapper.Map<Delivery>(@event);

            // Grpc call to Delivery service to get the Client, 
            // client is not added to event bus, because message will be very heavy
            var client = await _clientGrpcService.GetClientByIdAsync(delivery.ClientId.ToString());
            await _clientRepository.InsertClientAsync(client);

            await _deliveryService.InsertDeliveryAsync(delivery);
        }
    }
}
