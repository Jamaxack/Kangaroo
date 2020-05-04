using MediatR;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Delivering.API.Application.Commands
{
    public class CreateDeliveryCommand : IRequest<bool>
    {
        public Guid ClientId { get; set; }
        public decimal Price { get; set; }
        public short Weight { get; set; }
        public string Note { get; set; }
        public DeliveryLocationDTO PickUpLocation { get; set; }
        public DeliveryLocationDTO DropOffLocation { get; set; }

        public CreateDeliveryCommand()
        {

        }
    }
}
