using MediatR;
using Microsoft.Extensions.Logging;
using Delivering.Domain.AggregatesModel.DeliveryOrderAggregate;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Delivering.API.Application.Commands
{
    public class CreateDeliveryOrderCommandHandler : IRequestHandler<CreateDeliveryOrderCommand, bool>
    {
        private readonly IDeliveryOrderRepository _deliveryOrderRepository;
        private readonly ILogger<CreateDeliveryOrderCommandHandler> _logger;

        public CreateDeliveryOrderCommandHandler(IDeliveryOrderRepository deliveryOrderRepository, ILogger<CreateDeliveryOrderCommandHandler> logger)
        {
            _deliveryOrderRepository = deliveryOrderRepository ?? throw new ArgumentNullException(nameof(deliveryOrderRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Handle(CreateDeliveryOrderCommand message, CancellationToken cancellationToken)
        {
            var deliveryOrder = new DeliveryOrder(message.ClientId, message.Price, message.Weight, message.Note);
            deliveryOrder.SetPickUpLocation(message.PickUpLocation.GetDeliveryLocation());
            deliveryOrder.SetDropOffLocation(message.DropOffLocation.GetDeliveryLocation());

            _logger.LogInformation("----- Creating DeliveryOrder: {@DeliveryOrder}", deliveryOrder);

            _deliveryOrderRepository.Add(deliveryOrder);

            return await _deliveryOrderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}
