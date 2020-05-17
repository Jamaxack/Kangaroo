using System;

namespace Delivery.API.Application.Queries
{
    public class ClientViewModel
    {
        public Guid Id { get; set; }
        public string Phone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
