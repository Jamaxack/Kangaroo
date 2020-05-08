namespace Courier.API.Infrastructure.Repositories
{
    using Courier.API.Model;
    using Microsoft.Extensions.Options;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class CourierRepository : ICourierRepository
    {
        readonly CourierContext _courierContext;
        public CourierRepository(IOptions<CourierSettings> settings)
        {
            _courierContext = new CourierContext(settings);
        }

        public Task InsertCourierLocationAsync(CourierLocation courierLocation)
        {
            return _courierContext.CourierLocations.InsertOneAsync(courierLocation);
        }

        public Task InsertCourierAsync(Courier courier)
        {
            return _courierContext.Couriers.InsertOneAsync(courier);
        }


        public Task UpdateCourierAsync(Courier courier)
        {
            return _courierContext.Couriers.ReplaceOneAsync(
                doc => doc.Id == courier.Id,
                courier,
                new ReplaceOptions { IsUpsert = true });
        }

        public Task InsertDeliveryAsync(Delivery delivery)
        {
            return _courierContext.Deliveries.InsertOneAsync(delivery);
        }

        public Task<Courier> GetCourierByIdAsync(Guid courierId)
        {
            var filter = Builders<Courier>.Filter.Eq("_id", courierId);
            return _courierContext.Couriers
                .Find(filter)
                .FirstOrDefaultAsync();
        }

        public Task<List<Courier>> GetCouriersAsync()
        {
            return _courierContext.Couriers.Find(new BsonDocument()).ToListAsync(); ;
        }

        public Task<CourierLocation> GetCurrentCourierLocationByCourierIdAsync(Guid courierId)
        {
            var filter = Builders<CourierLocation>.Filter.Eq("CourierId", courierId);
            return _courierContext.CourierLocations
                .Find(filter)
                .FirstOrDefaultAsync();
        }

        public Task<List<Delivery>> GetDeliveriesByCourierIdAsync(Guid courierId)
        {
            var filter = Builders<Delivery>.Filter.Eq("CourierId", courierId);
            return _courierContext.Deliveries
                .Find(filter)
                .ToListAsync();
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
    }
}
