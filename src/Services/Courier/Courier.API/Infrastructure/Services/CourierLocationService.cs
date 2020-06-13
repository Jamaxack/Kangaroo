using System.Threading.Tasks;
using AutoMapper;
using Courier.API.DataTransferableObjects;
using Courier.API.Infrastructure.Repositories;
using Courier.API.Model;
using Kangaroo.Common.Facades;

namespace Courier.API.Infrastructure.Services
{
    public class CourierLocationService : ICourierLocationService
    {
        private readonly ICourierLocationRepository _courierLocationRepository;
        private readonly IDateTimeFacade _dateTimeFacade;
        private readonly IMapper _mapper;

        public CourierLocationService(ICourierLocationRepository courierLocationRepository, IMapper mapper,
            IDateTimeFacade dateTimeFacade)
        {
            _courierLocationRepository = courierLocationRepository;
            _mapper = mapper;
            _dateTimeFacade = dateTimeFacade;
        }

        public Task InsertCourierLocationAsync(CourierLocationDtoSave courierLocationDtoSave)
        {
            var courierLocation = _mapper.Map<CourierLocation>(courierLocationDtoSave);
            courierLocation.DateTime = _dateTimeFacade.UtcNow;
            return _courierLocationRepository.InsertCourierLocationAsync(courierLocation);
        }
    }
}