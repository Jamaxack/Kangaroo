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
        private readonly IDeliveryRepository _DeliveryRepository;
        private readonly ILogger<CreateDeliveryCommandHandler> _logger;

        public CreateDeliveryCommandHandler(IDeliveryRepository DeliveryRepository, ILogger<CreateDeliveryCommandHandler> logger)
        {
            _DeliveryRepository = DeliveryRepository ?? throw new ArgumentNullException(nameof(DeliveryRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Handle(CreateDeliveryCommand message, CancellationToken cancellationToken)
        {
            var Delivery = new Delivery(message.ClientId, message.Price, message.Weight, message.Note);
            Delivery.SetPickUpLocation(message.PickUpLocation.GetDeliveryLocation());
            Delivery.SetDropOffLocation(message.DropOffLocation.GetDeliveryLocation());

            _logger.LogInformation("----- Creating Delivery: {@Delivery}", Delivery);

            _DeliveryRepository.Add(Delivery);

            return await _DeliveryRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}
