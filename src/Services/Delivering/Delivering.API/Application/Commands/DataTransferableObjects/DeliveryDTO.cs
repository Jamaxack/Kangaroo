using Delivering.Domain.AggregatesModel.DeliveryAggregate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Delivering.API.Application.Commands
{
    public class DeliveryDTO
    {
        public long Number { get; set; }
        public short Weight { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? FinishedDateTime { get; set; }
        public decimal Price { get; set; }
        public string Note { get; set; }
        public DeliveryStatus DeliveryStatus { get; set; }
        public Guid ClientId { get; set; }
        public Guid? CourierId { get; set; }
        public DeliveryLocationDTO PickUpLocation { get; set; }
        public DeliveryLocationDTO DropOffLocation { get; set; }


        public static DeliveryDTO FromDelivery(Delivery Delivery)
        {
            return new DeliveryDTO()
            {
                Number = Delivery.Number,
                Weight = Delivery.Weight,
                CreatedDateTime = Delivery.CreatedDateTime,
                FinishedDateTime = Delivery.FinishedDateTime,
                Price = Delivery.Price,
                Note = Delivery.Note,
                DeliveryStatus = Delivery.DeliveryStatus,
                ClientId = Delivery.GetClientId,
                CourierId = Delivery.GetCourierId,
                DropOffLocation = DeliveryLocationDTO.FromLocation(Delivery.DropOffLocation),
                PickUpLocation = DeliveryLocationDTO.FromLocation(Delivery.PickUpLocation),
            };
        }
    }
}
