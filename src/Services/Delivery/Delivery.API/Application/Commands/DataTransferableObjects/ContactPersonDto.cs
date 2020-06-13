using Delivery.Domain.AggregatesModel.DeliveryAggregate;

namespace Delivery.API.Application.Commands.DataTransferableObjects
{
    public class ContactPersonDto
    {
        public string Name { get; set; }
        public string Phone { get; set; }

        public static ContactPersonDto FromContactPerson(ContactPerson contactPerson)
        {
            return new ContactPersonDto {Name = contactPerson.Name, Phone = contactPerson.Phone};
        }
    }
}