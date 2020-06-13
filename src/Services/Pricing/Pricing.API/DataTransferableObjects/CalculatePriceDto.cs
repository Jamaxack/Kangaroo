namespace Pricing.API.DataTransferableObjects
{
    public class CalculatePriceDto
    {
        public short Weight { get; set; }
        public DeliveryLocationDto PickUpLocation { get; set; }
        public DeliveryLocationDto DropOffLocation { get; set; }
    }
}