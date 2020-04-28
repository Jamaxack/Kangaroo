using MediatR;
using Microsoft.Extensions.Logging;
using Order.Domain.AggregatesModel.DeliveryOrderAggregate;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Order.API.Application.Commands
{

    public class DeleteDeliveryLocationCommandHandler : IRequestHandler<DeleteDeliveryLocationCommand, bool>
    {
        private readonly IDeliveryOrderRepository _deliveryOrderRepository;
        private readonly ILogger<DeleteDeliveryLocationCommand> _logger;


        public DeleteDeliveryLocationCommandHandler(IDeliveryOrderRepository deliveryOrderRepository, ILogger<DeleteDeliveryLocationCommand> logger)
        {
            _deliveryOrderRepository = deliveryOrderRepository ?? throw new ArgumentNullException(nameof(deliveryOrderRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Handle(DeleteDeliveryLocationCommand message, CancellationToken cancellationToken)
        {
            var deliveryOrder = await _deliveryOrderRepository.GetAsync(message.DeliveryOrderId);
            if (deliveryOrder == null)
                return false;

            var deliveryLocationToRemove = deliveryOrder.DeliveryLocations.FirstOrDefault(x => x.Id == message.DeliveryLocationId);
            if (deliveryLocationToRemove == null)
                return false;

            _logger.LogInformation("----- Deleting DeliveryLocation: {@DeliveryLocation}", deliveryLocationToRemove);

            deliveryOrder.RemoveDeliveryLocation(deliveryLocationToRemove);

            return await _deliveryOrderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}
