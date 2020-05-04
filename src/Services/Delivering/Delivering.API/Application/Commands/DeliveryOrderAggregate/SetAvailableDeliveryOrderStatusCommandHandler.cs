using MediatR;
using Microsoft.Extensions.Logging;
using Delivering.Domain.AggregatesModel.DeliveryOrderAggregate;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Delivering.API.Application.Commands
{
    public class SetAvailableDeliveryOrderStatusCommandHandler : IRequestHandler<SetAvailableDeliveryOrderStatusCommand, bool>
    {
        private readonly IDeliveryOrderRepository _deliveryOrderRepository;
        private readonly ILogger<SetAvailableDeliveryOrderStatusCommand> _logger;

        public SetAvailableDeliveryOrderStatusCommandHandler(IDeliveryOrderRepository deliveryOrderRepository, ILogger<SetAvailableDeliveryOrderStatusCommand> logger)
        {
            _deliveryOrderRepository = deliveryOrderRepository ?? throw new ArgumentNullException(nameof(deliveryOrderRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Handle(SetAvailableDeliveryOrderStatusCommand message, CancellationToken cancellationToken)
        {
            var deliveryOrder = await _deliveryOrderRepository.GetAsync(message.DeliveryOrderId);
            if (deliveryOrder == null)
                return false;

            _logger.LogInformation("----- Setting DeliveryOrder status to Available: {@DeliveryOrder}", deliveryOrder);

            deliveryOrder.SetDeliveryOrderAvailableStatus();

            return await _deliveryOrderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}
