using MediatR;
using Microsoft.Extensions.Logging;
using Delivering.Domain.AggregatesModel.DeliveryAggregate;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Delivering.API.Application.Commands
{
    public class SetAvailableDeliveryStatusCommandHandler : IRequestHandler<SetAvailableDeliveryStatusCommand, bool>
    {
        private readonly IDeliveryRepository _DeliveryRepository;
        private readonly ILogger<SetAvailableDeliveryStatusCommand> _logger;

        public SetAvailableDeliveryStatusCommandHandler(IDeliveryRepository DeliveryRepository, ILogger<SetAvailableDeliveryStatusCommand> logger)
        {
            _DeliveryRepository = DeliveryRepository ?? throw new ArgumentNullException(nameof(DeliveryRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Handle(SetAvailableDeliveryStatusCommand message, CancellationToken cancellationToken)
        {
            var Delivery = await _DeliveryRepository.GetAsync(message.DeliveryId);
            if (Delivery == null)
                return false;

            _logger.LogInformation("----- Setting Delivery status to Available: {@Delivery}", Delivery);

            Delivery.SetDeliveryAvailableStatus();

            return await _DeliveryRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}
