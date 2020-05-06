using Courier.API.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Courier.API.Infrastructure
{
    public class CourierContext
    {
        private readonly IMongoDatabase _database = null;

        public CourierContext(IOptions<CourierSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<Model.Courier> Couriers
        {
            get
            {
                return _database.GetCollection<Model.Courier>("Couriers");
            }
        }

        public IMongoCollection<CourierLocation> CourierLocations
        {
            get
            {
                return _database.GetCollection<CourierLocation>("CourierLocations");
            }
        }

        public IMongoCollection<Delivery> Deliveries
        {
            get
            {
                return _database.GetCollection<Delivery>("Deliveries");
            }
        }


    }
}