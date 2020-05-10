using Courier.API.Model;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Courier.API.Infrastructure.Repositories
{
    public class CourierLocationRepository : ICourierLocationRepository
    {
        readonly CourierContext _courierContext;

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
