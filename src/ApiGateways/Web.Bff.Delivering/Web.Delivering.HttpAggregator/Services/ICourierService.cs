using System.Threading.Tasks;
using Web.Delivering.HttpAggregator.Models;

namespace Web.Delivering.HttpAggregator.Services
{
    public interface ICourierService
    {
        Task<Delivery> GetDeliveryByIdAsync(string deliveryId);
    }
}
