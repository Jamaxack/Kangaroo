using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Courier.API.DataTransferableObjects;

namespace Courier.API.Infrastructure.Services
{
    public interface IDeliveryService
    {
        Task<List<DeliveryDto>> GetAvailableDeliveriesAsync();
        Task<List<DeliveryDto>> GetDeliveriesByCourierIdAsync(Guid courierId); 
        Task AssignCourierToDeliveryAsync(AssignCourierToDeliveryDtoSave assignCourierToDelivery);
        Task InsertDeliveryAsync(DeliveryDtoSave delivery);
        Task<DeliveryDto> GetDeliveryByIdAsync(Guid deliveryId);
        Task DeleteDeliveryByIdAsync(Guid deliveryId);
        Task SetDeliveryStatusToCourierPickedUpAsync(Guid deliveryId);
    }
}