using MediatR;
using Microsoft.Extensions.Logging;
using DeliveryOrder.Domain.AggregatesModel.DeliveryOrderAggregate;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryOrder.API.Application.Commands
{
    public class CreateDeliveryOrderCommandHandler : IRequestHandler<CreateDeliveryOrderCommand, bool>
    {
        private readonly IDeliveryOrderRepository _deliveryOrderRepository;
        private readonly ILogger<CreateDeliveryOrderCommandHandler> _logger;

        public CreateDeliveryOrderCommandHandler(IDeliveryOrderRepository deliveryOrderRepository, ILogger<CreateDeliveryOrderCommandHandler> logger)
        {
            _deliveryOrderRepository = deliveryOrderRepository ?? throw new ArgumentNullException(nameof(deliveryOrderRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Handle(CreateDeliveryOrderCommand message, CancellationToken cancellationToken)
        {
            var settings = new DeliveryOrderNotificationSettings(message.ShouldNotifySenderOnDeliveryOrderStatusChange, message.ShouldNotifyRecipientOnDeliveryOrderStatusChange);
            var deliveryOrder = new Domain.AggregatesModel.DeliveryOrderAggregate.DeliveryOrder(message.ClientId, message.PaymentAmount, message.InsuranceAmount, message.Weight, message.Note, settings);

            foreach (var location in message.DeliveryLocations)
            {
                var contactPerson = new ContactPerson(location.ContactPersonName, location.ContactPersonPhone);
                var deliveryLocation = new DeliveryLocation(location.Address, location.BuildingNumber, location.EntranceNumber, location.FloorNumber, location.ApartmentNumber, location.Latitude, location.Longitude, location.Note, location.BuyoutAmount, location.TakingAmount, location.IsPaymentInThisDeliveryLocation, location.DeliveryLocationActionId, location.ArrivalStartDateTime, location.ArrivalFinishDateTime, location.CourierArrivedDateTime, contactPerson);

                deliveryOrder.AddDelivaryLocation(deliveryLocation);
            }

            _logger.LogInformation("----- Creating DeliveryOrder: {@DeliveryOrder}", deliveryOrder);

            _deliveryOrderRepository.Add(deliveryOrder);

            return await _deliveryOrderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}
