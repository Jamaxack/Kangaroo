using System;
using System.Threading;
using System.Threading.Tasks;
using Delivery.Domain.AggregatesModel.DeliveryAggregate;
using Kangaroo.Common.Facades;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Delivery.API.Application.Commands.DeliveryAggregate
{
    public class CreateDeliveryCommandHandler : IRequestHandler<CreateDeliveryCommand, bool>
    {
        private readonly IDateTimeFacade _dateTimeFacade;
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly ILogger<CreateDeliveryCommandHandler> _logger;

        public CreateDeliveryCommandHandler(IDeliveryRepository deliveryRepository,
            ILogger<CreateDeliveryCommandHandler> logger, IDateTimeFacade dateTimeFacade)
        {
            _deliveryRepository = deliveryRepository ?? throw new ArgumentNullException(nameof(deliveryRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dateTimeFacade = dateTimeFacade;
        }

        public async Task<bool> Handle(CreateDeliveryCommand message, CancellationToken cancellationToken)
        {
            var delivery = new Domain.AggregatesModel.DeliveryAggregate.Delivery(message.ClientId, message.Price,
                message.Weight, message.Note, _dateTimeFacade.UtcNow);
            delivery.SetPickUpLocation(message.PickUpLocation.GetDeliveryLocation());
            delivery.SetDropOffLocation(message.DropOffLocation.GetDeliveryLocation());

            _logger.LogInformation("----- Creating Delivery: {@Delivery}", delivery);

            _deliveryRepository.Add(delivery);

            return await _deliveryRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}