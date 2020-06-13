using MediatR;
using System;

namespace Delivery.API.Application.Commands
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
