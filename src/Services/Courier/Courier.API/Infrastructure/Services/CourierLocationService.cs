using AutoMapper;
using Courier.API.DataTransferableObjects;
using Courier.API.Infrastructure.Repositories;
using Courier.API.Model;
using Kangaroo.Common.Facades;
using System.Threading.Tasks;

namespace Courier.API.Infrastructure.Services
{
    public class CourierLocationService : ICourierLocationService
    {
        readonly ICourierLocationRepository _courierLocationRepository;
        readonly IMapper _mapper;
        readonly IDateTimeFacade _dateTimeFacade;

        public CourierLocationService(ICourierLocationRepository courierLocationRepository, IMapper mapper, IDateTimeFacade dateTimeFacade)
        {
            _courierLocationRepository = courierLocationRepository;
            _mapper = mapper;
            _dateTimeFacade = dateTimeFacade;
        }

        public Task InsertCourierLocationAsync(CourierLocationDTO courierLocationDTO)
        {
            var courierLocation = _mapper.Map<CourierLocation>(courierLocationDTO);
            courierLocation.DateTime = _dateTimeFacade.UtcNow;
            return _courierLocationRepository.InsertCourierLocationAsync(courierLocation);
        }
    }
}
