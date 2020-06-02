using Pricing.API.DataTransferableObjects;
using System.Threading.Tasks;

namespace Pricing.API.Infrastucture.Services
{
    public interface IPricingService
    {
        Task<PriceDTO> CalculatePriceAsync(CalculatePriceDTO calculatePriceDTO);
    }
}
