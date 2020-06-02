using Newtonsoft.Json;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace Pricing.API.Infrastucture.Repositories
{
    public class RedisPricingRepository : IPricingRepository
    {
        readonly IDatabase _database;

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
            => _database.StringSetAsync(key, JsonConvert.SerializeObject(value));
    }
}
