using Courier.API.Model;
using System;
using System.Threading.Tasks;

namespace Courier.API.Infrastructure.Repositories
{
    public interface IDeliveryRepository
    {
        Task InsertDeliveryAsync(Delivery delivery);
        Task<Delivery> GetDeliveryByIdAsync(Guid deliveryId);
        Task DeleteDeliveryByIdAsync(Guid deliveryId);
        Task SetDeliveryStatusToCourierPickedUpAsync(Guid deliveryId);
    }
}
