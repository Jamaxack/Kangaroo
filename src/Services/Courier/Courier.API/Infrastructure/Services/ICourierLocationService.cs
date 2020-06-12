using Courier.API.DataTransferableObjects;
using System.Threading.Tasks;

namespace Courier.API.Infrastructure.Services
{
    public interface ICourierLocationService
    {
        Task InsertCourierLocationAsync(CourierLocationDtoSave courierLocation);
    }
}
