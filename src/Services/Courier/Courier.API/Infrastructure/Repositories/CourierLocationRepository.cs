using System.Threading.Tasks;
using Courier.API.Model;
using Microsoft.Extensions.Options;

namespace Courier.API.Infrastructure.Repositories
{
    public class CourierLocationRepository : ICourierLocationRepository
    {
        private readonly CourierContext _courierContext;

        public CourierLocationRepository(IOptions<CourierSettings> settings)
        {
            _courierContext = new CourierContext(settings);
        }

        public Task InsertCourierLocationAsync(CourierLocation courierLocation)
        {
            return _courierContext.CourierLocations.InsertOneAsync(courierLocation);
        }
    }
}