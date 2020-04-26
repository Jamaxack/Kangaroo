using MediatR;
using Order.Domain.AggregatesModel.DeliveryOrderAggregate;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Order.API.Application.Commands
{
    public class CreateDeliveryOrderCommandHandler : IRequestHandler<CreateDeliveryOrderCommand, bool>
    {
        private readonly IDeliveryOrderRepository _deliveryOrderRepository;

        public CreateDeliveryOrderCommandHandler(IDeliveryOrderRepository deliveryOrderRepository)
        {
            _deliveryOrderRepository = deliveryOrderRepository ?? throw new ArgumentNullException(nameof(deliveryOrderRepository));
        }

        public async Task<bool> Handle(CreateDeliveryOrderCommand message, CancellationToken cancellationToken)
        {
            var settings = new DeliveryOrderNotificationSettings(message.ShouldNotifySenderOnOrderStatusChange, message.ShouldNotifyRecipientOnOrderStatusChange);
            var deliveryOrder = new DeliveryOrder(message.ClientId, message.PaymentAmount, message.InsuranceAmount, message.Weight, message.Note, settings);

            foreach (var location in message.DeliveryLocations)
            {
                var contactPerson = new ContactPerson(location.ContactPersonName, location.ContactPersonPhone);
                deliveryOrder.AddDelivaryLocation(location.Address, location.BuildingNumber, location.EnterenceNumber, location.FloorNumber, location.ApartmentNumber, location.Latitude, location.Longitude, location.Note, location.BuyoutAmount, location.TakingAmount, location.IsPaymentInThisDeliveryLocation, location.DeliveryLocationActionId, location.ArrivalStartDateTime, location.ArrivalFinishDateTime, location.CourierArrivedDateTime, contactPerson);
            }

            return await _deliveryOrderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}
