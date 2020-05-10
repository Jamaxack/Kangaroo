using Courier.API.Model;
using System.Threading.Tasks;

namespace Courier.API.Infrastructure.Repositories
{
    public interface ICourierLocationRepository
    {
        Task InsertCourierLocationAsync(CourierLocation courierLocation);
    }
}
