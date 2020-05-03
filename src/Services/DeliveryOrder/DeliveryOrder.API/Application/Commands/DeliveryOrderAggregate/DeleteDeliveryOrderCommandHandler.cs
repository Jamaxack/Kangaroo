using DeliveryOrder.Domain.AggregatesModel.DeliveryOrderAggregate;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryOrder.API.Application.Commands
{

    public class DeleteDeliveryOrderCommandHandler : IRequestHandler<DeleteDeliveryOrderCommand, bool>
    {
        private readonly IDeliveryOrderRepository _deliveryOrderRepository;
        private readonly ILogger<DeleteDeliveryOrderCommand> _logger;


        public DeleteDeliveryOrderCommandHandler(IDeliveryOrderRepository deliveryOrderRepository, ILogger<DeleteDeliveryOrderCommand> logger)
        {
            _deliveryOrderRepository = deliveryOrderRepository ?? throw new ArgumentNullException(nameof(deliveryOrderRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Handle(DeleteDeliveryOrderCommand message, CancellationToken cancellationToken)
        {
            var deliveryOrder = await _deliveryOrderRepository.GetAsync(message.DeliveryOrderId);
            if (deliveryOrder == null)
                return false;
             
            _logger.LogInformation("----- Deleting DeliveryOrder: {@DeliveryLocation}", deliveryOrder);

            _deliveryOrderRepository.Delete(deliveryOrder);

            return await _deliveryOrderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}
