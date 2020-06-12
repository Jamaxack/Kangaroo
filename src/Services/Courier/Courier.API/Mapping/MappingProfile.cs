using AutoMapper;
using Courier.API.DataTransferableObjects;
using Courier.API.IntegrationEvents.Events;

namespace Courier.API.Mapping
{
    using Model;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //DtoSave to Model 
            CreateMap<CourierDtoSave, Courier>();
            CreateMap<CourierLocationDtoSave, CourierLocation>();
            CreateMap<DeliveryDtoSave, Delivery>();
    
            //Model to Dto
            CreateMap<ContactPerson, ContactPersonDto>();
            CreateMap<Courier, CourierDto>();
            CreateMap<CourierLocation, CourierLocationDto>();
            CreateMap<Delivery, DeliveryDto>();
            CreateMap<DeliveryLocation, DeliveryLocationDto>();

            //Integration event to Model entity
            CreateMap<DeliveryCreatedIntegrationEvent, Delivery>();
        }
    }
}