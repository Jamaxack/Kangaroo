using Order.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Domain.AggregatesModel.CourierAggregate
{
    public class Courier : Entity, IAggregateRoot
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Phone { get; private set; }

        public Courier() { }

        public Courier(Guid id, string firstName, string lastName, string phone)
        {
            Id = id == Guid.Empty ? throw new ArgumentNullException(nameof(id)) : id;
            FirstName = string.IsNullOrWhiteSpace(firstName) ? throw new ArgumentNullException(nameof(firstName)) : firstName;
            LastName = string.IsNullOrWhiteSpace(lastName) ? throw new ArgumentNullException(nameof(lastName)) : lastName;
            Phone = string.IsNullOrWhiteSpace(phone) ? throw new ArgumentNullException(nameof(phone)) : phone;
        }
    }
}
