using Delivery.Domain.AggregatesModel.DeliveryAggregate;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Delivery.API.Application.Commands
{

    public class DeleteDeliveryCommandHandler : IRequestHandler<DeleteDeliveryCommand, bool>
    {
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly ILogger<DeleteDeliveryCommand> _logger;


        public DeleteDeliveryCommandHandler(IDeliveryRepository deliveryRepository, ILogger<DeleteDeliveryCommand> logger)
        {
            _deliveryRepository = deliveryRepository ?? throw new ArgumentNullException(nameof(deliveryRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Handle(DeleteDeliveryCommand message, CancellationToken cancellationToken)
        {
            var delivery = await _deliveryRepository.GetAsync(message.DeliveryId);
            if (delivery == null)
                return false;

            _logger.LogInformation("----- Deleting Delivery: {@DeliveryLocation}", delivery);

            _deliveryRepository.Delete(delivery);

            return await _deliveryRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}
