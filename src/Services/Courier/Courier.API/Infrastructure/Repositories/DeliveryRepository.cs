using Courier.API.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace Courier.API.Infrastructure.Repositories
{
    public class DeliveryRepository : IDeliveryRepository
    {
        readonly CourierContext _courierContext;

        public DeliveryRepository(IOptions<CourierSettings> settings)
        {
            _courierContext = new CourierContext(settings);
        }

        public Task InsertDeliveryAsync(Delivery delivery)
        {
            return _courierContext.Deliveries.InsertOneAsync(delivery);
        }

        public Task<Delivery> GetDeliveryByIdAsync(Guid deliveryId)
        {
            var filter = Builders<Delivery>.Filter.Eq("_id", deliveryId);
            return _courierContext.Deliveries
                .Find(filter)
                .FirstOrDefaultAsync();
        }

        public Task DeleteDeliveryByIdAsync(Guid deliveryId)
        {
            var deleteFilter = Builders<Delivery>.Filter.Eq("_id", deliveryId);
            return _courierContext.Deliveries.DeleteOneAsync(deleteFilter);
        }

        public Task SetDeliveryStatusToCourierPickedUpAsync(Guid deliveryId)
        {
            var filter = Builders<Delivery>.Filter.Eq("_id", deliveryId);
            var update = Builders<Delivery>.Update.Set(delivery => delivery.DeliveryStatus, DeliveryStatus.CourierPickedUp);
            var options = new FindOneAndUpdateOptions<Delivery>();
            return _courierContext.Deliveries.FindOneAndUpdateAsync(filter, update, options);
        }
    }
}
