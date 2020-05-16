namespace Delivery.API.Application.Commands
{
    using Delivery.Domain.AggregatesModel.DeliveryAggregate;
    using System;

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


        public static DeliveryDTO FromDelivery(Delivery delivery)
        {
            return new DeliveryDTO()
            {
                Number = delivery.Number,
                Weight = delivery.Weight,
                CreatedDateTime = delivery.CreatedDateTime,
                FinishedDateTime = delivery.FinishedDateTime,
                Price = delivery.Price,
                Note = delivery.Note,
                DeliveryStatus = delivery.DeliveryStatus,
                ClientId = delivery.GetClientId,
                CourierId = delivery.GetCourierId,
                DropOffLocation = DeliveryLocationDTO.FromLocation(delivery.DropOffLocation),
                PickUpLocation = DeliveryLocationDTO.FromLocation(delivery.PickUpLocation),
            };
        }
    }
}
