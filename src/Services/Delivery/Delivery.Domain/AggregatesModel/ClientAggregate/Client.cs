using System;
using Delivery.Domain.Common;

namespace Delivery.Domain.AggregatesModel.ClientAggregate
{
    public class Client : Entity, IAggregateRoot
    {
        public Client()
        {
        }

        public Client(Guid id, string firstName, string lastName, string phone)
        {
            Id = id == Guid.Empty ? throw new ArgumentNullException(nameof(id)) : id;
            FirstName = string.IsNullOrWhiteSpace(firstName)
                ? throw new ArgumentNullException(nameof(firstName))
                : firstName;
            LastName = string.IsNullOrWhiteSpace(lastName)
                ? throw new ArgumentNullException(nameof(lastName))
                : lastName;
            Phone = string.IsNullOrWhiteSpace(phone) ? throw new ArgumentNullException(nameof(phone)) : phone;
        }

        public string FirstName { get; }
        public string LastName { get; }
        public string Phone { get; }
    }
}