using System.Threading.Tasks;
using Pricing.API.DataTransferableObjects;

namespace Pricing.API.Infrastucture.Services
{
    public interface IPricingService
    {
        Task<PriceDto> CalculatePriceAsync(CalculatePriceDto calculatePriceDto);
    }
}