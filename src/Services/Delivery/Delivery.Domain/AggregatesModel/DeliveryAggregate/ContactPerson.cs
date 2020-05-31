using Delivery.Domain.Common;
using System;
using System.Collections.Generic;

namespace Delivery.Domain.AggregatesModel.DeliveryAggregate
{
    /// <summary>
    /// Sender or Recipient person
    /// </summary>
    public class ContactPerson : ValueObject
    {
        public string Name { get; private set; }
        public string Phone { get; private set; }

        public ContactPerson() { }

        public ContactPerson(string name, string phone)
        {
            Name = name;
            Phone = string.IsNullOrWhiteSpace(phone) ? throw new ArgumentNullException(nameof(phone)) : phone;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
            yield return Phone;
        }
    }
}
