using System;
using System.Collections.Generic;
using Delivery.Domain.Common;

namespace Delivery.Domain.AggregatesModel.DeliveryAggregate
{
    /// <summary>
    ///     Sender or Recipient person
    /// </summary>
    public class ContactPerson : ValueObject
    {
        public ContactPerson()
        {
        }

        public ContactPerson(string name, string phone)
        {
            Name = name;
            Phone = string.IsNullOrWhiteSpace(phone) ? throw new ArgumentNullException(nameof(phone)) : phone;
        }

        public string Name { get; }
        public string Phone { get; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
            yield return Phone;
        }
    }
}