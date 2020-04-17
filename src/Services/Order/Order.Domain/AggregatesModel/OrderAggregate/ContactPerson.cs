﻿using Order.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Domain.AggregatesModel.OrderAggregate
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
            Phone = phone;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
            yield return Phone;
        }
    }
}
