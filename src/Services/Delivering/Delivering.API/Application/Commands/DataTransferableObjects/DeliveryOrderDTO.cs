using Delivering.Domain.AggregatesModel.DeliveryOrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Delivering.API.Application.Commands
{
    public class DeliveryOrderDTO
    {
        public long Number { get; set; }
        public short Weight { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? FinishedDateTime { get; set; }
        public decimal Price { get; set; }
        public string Note { get; set; }
        public DeliveryOrderStatus DeliveryOrderStatus { get; set; }
        public Guid ClientId { get; set; }
        public Guid? CourierId { get; set; }
        public DeliveryLocationDTO PickUpLocation { get; set; }
        public DeliveryLocationDTO DropOffLocation { get; set; }


        public static DeliveryOrderDTO FromDeliveryOrder(DeliveryOrder deliveryOrder)
        {
            return new DeliveryOrderDTO()
            {
                Number = deliveryOrder.Number,
                Weight = deliveryOrder.Weight,
                CreatedDateTime = deliveryOrder.CreatedDateTime,
                FinishedDateTime = deliveryOrder.FinishedDateTime,
                Price = deliveryOrder.Price,
                Note = deliveryOrder.Note,
                DeliveryOrderStatus = deliveryOrder.DeliveryOrderStatus,
                ClientId = deliveryOrder.GetClientId,
                CourierId = deliveryOrder.GetCourierId,
                DropOffLocation = DeliveryLocationDTO.FromLocation(deliveryOrder.DropOffLocation),
                PickUpLocation = DeliveryLocationDTO.FromLocation(deliveryOrder.PickUpLocation),
            };
        }
    }
}
