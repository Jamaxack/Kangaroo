namespace Pricing.API.DataTransferableObjects
{
    public class CalculatePriceDTO
    {
        public short Weight { get; set; }
        public DeliveryLocationDTO PickUpLocation { get; set; }
        public DeliveryLocationDTO DropOffLocation { get; set; }
    }
}
