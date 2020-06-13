using System.Threading.Tasks;
using Courier.API.DataTransferableObjects;

namespace Courier.API.Infrastructure.Services
{
    public interface ICourierLocationService
    {
        Task InsertCourierLocationAsync(CourierLocationDtoSave courierLocation);
    }
}