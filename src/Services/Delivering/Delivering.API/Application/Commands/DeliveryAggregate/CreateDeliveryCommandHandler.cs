using MediatR;
using Microsoft.Extensions.Logging;
using Delivering.Domain.AggregatesModel.DeliveryAggregate;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Delivering.API.Application.Commands
{
    public class CreateDeliveryCommandHandler : IRequestHandler<CreateDeliveryCommand, bool>
    {
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly ILogger<CreateDeliveryCommandHandler> _logger;

        public CreateDeliveryCommandHandler(IDeliveryRepository deliveryRepository, ILogger<CreateDeliveryCommandHandler> logger)
        {
            _deliveryRepository = deliveryRepository ?? throw new ArgumentNullException(nameof(deliveryRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Handle(CreateDeliveryCommand message, CancellationToken cancellationToken)
        {
            var delivery = new Delivery(message.ClientId, message.Price, message.Weight, message.Note);
            delivery.SetPickUpLocation(message.PickUpLocation.GetDeliveryLocation());
            delivery.SetDropOffLocation(message.DropOffLocation.GetDeliveryLocation());

            _logger.LogInformation("----- Creating Delivery: {@Delivery}", delivery);

            _deliveryRepository.Add(delivery);

            return await _deliveryRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}
