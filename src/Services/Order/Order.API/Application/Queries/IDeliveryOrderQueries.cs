using System;
using System.Threading.Tasks;

namespace Order.API.Application.Queries
{
    public interface IDeliveryOrderQueries
    {
        Task<DeliveryOrderViewModel> GetDeliveryOrderByIdAsync(Guid id); 
    }
}
