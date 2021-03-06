﻿using System.Threading.Tasks;
using AutoMapper;
using Courier.API.DataTransferableObjects;
using Courier.API.Infrastructure.GrpcServices;
using Courier.API.Infrastructure.Repositories;
using Courier.API.Infrastructure.Services;
using Courier.API.IntegrationEvents.Events;
using Courier.API.Model;
using Kangaroo.BuildingBlocks.EventBus.Abstractions;
using Microsoft.Extensions.Logging;

namespace Courier.API.IntegrationEvents.EventHandling
{
    public class DeliveryCreatedIntegrationEventHandler : IIntegrationEventHandler<DeliveryCreatedIntegrationEvent>
    {
        private readonly IClientGrpcService _clientGrpcService;
        private readonly IClientRepository _clientRepository;
        private readonly IDeliveryService _deliveryService;
        private readonly ILogger<DeliveryCreatedIntegrationEventHandler> _logger;
        private readonly IMapper _mapper;

        public DeliveryCreatedIntegrationEventHandler(IDeliveryService deliveryService,
            IClientGrpcService clientGrpcService, IClientRepository clientRepository, IMapper mapper,
            ILogger<DeliveryCreatedIntegrationEventHandler> logger)
        {
            _deliveryService = deliveryService;
            _clientGrpcService = clientGrpcService;
            _clientRepository = clientRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Handle(DeliveryCreatedIntegrationEvent @event)
        {
            _logger.LogInformation(
                "----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})",
                @event.Id, Program.AppName, @event);
            var delivery = _mapper.Map<Delivery>(@event);

            var existingClient = _clientRepository.GetClientByIdAsync(delivery.ClientId);
            if (existingClient == null)
            {
                // Grpc call to Delivery service to get the Client, 
                // client is not added to event bus, because message will be very heavy
                var client = await _clientGrpcService.GetClientByIdAsync(delivery.ClientId.ToString());
                await _clientRepository.InsertClientAsync(client);
            }

            var deliveryDtoSave = _mapper.Map<DeliveryDtoSave>(delivery);
            await _deliveryService.InsertDeliveryAsync(deliveryDtoSave);
        }
    }
}