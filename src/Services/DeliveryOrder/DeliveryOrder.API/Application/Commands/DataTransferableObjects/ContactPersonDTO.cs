using DeliveryOrder.Domain.AggregatesModel.DeliveryOrderAggregate;

namespace DeliveryOrder.API.Application.Commands
{
    public class ContactPersonDTO
    {
        public string Name { get; set; }
        public string Phone { get; set; }

        public static ContactPersonDTO FromContactPerson(ContactPerson contactPerson)
        {
            return new ContactPersonDTO() { Name = contactPerson.Name, Phone = contactPerson.Phone };
        }
    }
}
