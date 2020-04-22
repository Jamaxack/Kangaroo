using System;
using System.Threading.Tasks;

namespace Order.API.Application.Queries
{
    public interface IDeliveryOrderQueries
    {
        Task<DeliveryOrder> GetDeliveryOrderAsync(Guid id); 
    }
}
