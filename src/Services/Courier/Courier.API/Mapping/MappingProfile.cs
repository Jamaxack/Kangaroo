using AutoMapper;
using Courier.API.DataTransferableObjects;
using Courier.API.Model;

namespace Courier.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //DTO to Model
            CreateMap<CourierLocationDTO, CourierLocation>();
        }
    }
}
