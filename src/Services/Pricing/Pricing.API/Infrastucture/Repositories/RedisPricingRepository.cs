using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Pricing.API.Infrastucture.Repositories
{
    public class RedisPricingRepository : IPricingRepository
    {
        private readonly IDatabase _database;

        public RedisPricingRepository(ConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task<Model.Pricing> GetPricingAsync(string key)
        {
            var data = await _database.StringGetAsync(key);
            if (data.IsNullOrEmpty)
                return null;

            return JsonConvert.DeserializeObject<Model.Pricing>(data);
        }

        public Task InsertPricingAsync(string key, Model.Pricing value)
        {
            return _database.StringSetAsync(key, JsonConvert.SerializeObject(value));
        }
    }
}