using Delivering.Domain.AggregatesModel.DeliveryAggregate;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Delivering.API.Application.Commands
{

    public class DeleteDeliveryCommandHandler : IRequestHandler<DeleteDeliveryCommand, bool>
    {
        private readonly IDeliveryRepository _DeliveryRepository;
        private readonly ILogger<DeleteDeliveryCommand> _logger;


        public DeleteDeliveryCommandHandler(IDeliveryRepository DeliveryRepository, ILogger<DeleteDeliveryCommand> logger)
        {
            _DeliveryRepository = DeliveryRepository ?? throw new ArgumentNullException(nameof(DeliveryRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Handle(DeleteDeliveryCommand message, CancellationToken cancellationToken)
        {
            var Delivery = await _DeliveryRepository.GetAsync(message.DeliveryId);
            if (Delivery == null)
                return false;

            _logger.LogInformation("----- Deleting Delivery: {@DeliveryLocation}", Delivery);

            _DeliveryRepository.Delete(Delivery);

            return await _DeliveryRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}
