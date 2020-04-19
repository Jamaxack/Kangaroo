using Order.Domain.Common;
using Order.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Order.Domain.AggregatesModel.DeliveryOrderAggregate
{
    public class DeliveryLocationAction : Enumeration
    { 
        public static DeliveryLocationAction PickUp = new DeliveryLocationAction(1, nameof(PickUp).ToLowerInvariant()); 
        public static DeliveryLocationAction DropOff = new DeliveryLocationAction(2, nameof(DropOff).ToLowerInvariant()); 

        public DeliveryLocationAction(short id, string name)
            : base(id, name)
        { }

        public static IEnumerable<DeliveryLocationAction> List() => new[] { PickUp, DropOff};

        public static DeliveryLocationAction FromName(string name)
        {
            var state = List().SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
                throw new OrderingDomainException($"Possible values for DeliveryLocationActions: {String.Join(",", List().Select(s => s.Name))}");

            return state;
        }

        public static DeliveryLocationAction From(short id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
                throw new OrderingDomainException($"Possible values for DeliveryLocationActions: {String.Join(",", List().Select(s => s.Name))}");

            return state;
        }
    }
}
