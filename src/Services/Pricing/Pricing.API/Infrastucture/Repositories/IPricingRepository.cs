using System.Threading.Tasks;

namespace Pricing.API.Infrastucture.Repositories
{
    public interface IPricingRepository
    {
        Task<Model.Pricing> GetPricingAsync(string key);

        Task InsertPricingAsync(string key, Model.Pricing value);
    }
}
