using System;

namespace Courier.API.DataTransferableObjects
{
    public class CourierDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
    }
}