using System.Threading.Tasks;
using Courier.API.Model;

namespace Courier.API.Infrastructure.Repositories
{
    public interface ICourierLocationRepository
    {
        Task InsertCourierLocationAsync(CourierLocation courierLocation);
    }
}