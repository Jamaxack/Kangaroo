using Courier.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Courier.API.Infrastructure.Services
{
    public interface ICourierLocationService
    {
        Task InsertCourierLocationAsync(CourierLocation courierLocation);
    }
}
