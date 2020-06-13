using System;
using Delivery.API.Application.Commands.DataTransferableObjects;
using MediatR;

namespace Delivery.API.Application.Commands.DeliveryAggregate
{
    public class CreateDeliveryCommand : IRequest<bool>
    {
        public Guid ClientId { get; set; }
        public decimal Price { get; set; }
        public short Weight { get; set; }
        public string Note { get; set; }
        public DeliveryLocationDto PickUpLocation { get; set; }
        public DeliveryLocationDto DropOffLocation { get; set; }

        public CreateDeliveryCommand()
        {

        }
    }
}
