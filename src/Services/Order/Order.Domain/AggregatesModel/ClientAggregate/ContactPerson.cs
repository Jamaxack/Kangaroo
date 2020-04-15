using Order.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Domain.AggregatesModel.ClientAggregate
{
    /// <summary>
    /// Sender or Recipient person
    /// </summary>
    public class ContactPerson : ValueObject
    {
        public ContactPerson()
        {
        }

        public string Name { get; set; }
        public string Phone { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
