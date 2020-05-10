using Courier.API.Model;
using System;
using System.Threading.Tasks;

namespace Courier.API.Infrastructure.Services
{
    public interface IDeliveryService
    {
        Task InsertDeliveryAsync(Delivery delivery);
        Task<Delivery> GetDeliveryByIdAsync(Guid deliveryId);
        Task DeleteDeliveryByIdAsync(Guid deliveryId);
        Task DeliveryStatusChangedToCourierPickedUp(Guid deliveryId);
    }
}
