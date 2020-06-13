using System;
using System.Threading;
using System.Threading.Tasks;
using Delivery.Domain.AggregatesModel.DeliveryAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Delivery.API.Application.Commands.DeliveryAggregate
{
    public class SetAvailableDeliveryStatusCommandHandler : IRequestHandler<SetAvailableDeliveryStatusCommand, bool>
    {
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly ILogger<SetAvailableDeliveryStatusCommand> _logger;

        public SetAvailableDeliveryStatusCommandHandler(IDeliveryRepository deliveryRepository,
            ILogger<SetAvailableDeliveryStatusCommand> logger)
        {
            _deliveryRepository = deliveryRepository ?? throw new ArgumentNullException(nameof(deliveryRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Handle(SetAvailableDeliveryStatusCommand message, CancellationToken cancellationToken)
        {
            var delivery = await _deliveryRepository.GetAsync(message.DeliveryId);
            if (delivery == null)
                return false;

            _logger.LogInformation("----- Setting Delivery status to Available: {@Delivery}", delivery);

            delivery.SetDeliveryAvailableStatus();

            return await _deliveryRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}