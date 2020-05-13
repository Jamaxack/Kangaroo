using AutoMapper;
using Courier.API.DataTransferableObjects;
using Courier.API.IntegrationEvents.Events;
using Courier.API.Model;

namespace Courier.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //DTO to Model
            CreateMap<CourierLocationDTO, CourierLocation>();

            //Integration event to Model entity
            CreateMap<DeliveryCreatedIntegrationEvent, Delivery>();
        }
    }
}
