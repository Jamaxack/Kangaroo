namespace Pricing.API.DataTransferableObjects
{
    public class PriceDto
    {
        public decimal Price { get; set; }
        public double Distance { get; set; }
        public double Duration { get; set; }
    }
}