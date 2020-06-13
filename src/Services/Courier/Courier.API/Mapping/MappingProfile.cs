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
            //DtoSave to Model 
            CreateMap<CourierDtoSave, Model.Courier>();
            CreateMap<CourierLocationDtoSave, CourierLocation>();
            CreateMap<DeliveryDtoSave, Delivery>();

            //Model to Dto
            CreateMap<ContactPerson, ContactPersonDto>();
            CreateMap<Model.Courier, CourierDto>();
            CreateMap<CourierLocation, CourierLocationDto>();
            CreateMap<Delivery, DeliveryDto>();
            CreateMap<DeliveryLocation, DeliveryLocationDto>();

            //Integration event to Model entity
            CreateMap<DeliveryCreatedIntegrationEvent, Delivery>();
        }
    }
}