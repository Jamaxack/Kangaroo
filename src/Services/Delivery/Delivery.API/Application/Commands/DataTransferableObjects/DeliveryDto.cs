namespace Delivery.API.Application.Commands
{
    using Delivery.Domain.AggregatesModel.DeliveryAggregate;
    using System;

    public class DeliveryDto
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
        public DeliveryLocationDto PickUpLocation { get; set; }
        public DeliveryLocationDto DropOffLocation { get; set; }


        public static DeliveryDto FromDelivery(Delivery delivery)
        {
            return new DeliveryDto()
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
                DropOffLocation = DeliveryLocationDto.FromLocation(delivery.DropOffLocation),
                PickUpLocation = DeliveryLocationDto.FromLocation(delivery.PickUpLocation),
            };
        }
    }
}
