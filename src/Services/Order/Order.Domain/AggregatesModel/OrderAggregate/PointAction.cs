using Order.Domain.Common;
using Order.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Order.Domain.AggregatesModel.OrderAggregate
{
    public class PointAction : Enumeration
    { 
        public static PointAction PickUp = new PointAction(1, nameof(PickUp).ToLowerInvariant()); 
        public static PointAction DropOff = new PointAction(2, nameof(DropOff).ToLowerInvariant()); 

        public PointAction(short id, string name)
            : base(id, name)
        { }

        public static IEnumerable<PointAction> List() => new[] { PickUp, DropOff};

        public static PointAction FromName(string name)
        {
            var state = List().SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
                throw new OrderingDomainException($"Possible values for PointActions: {String.Join(",", List().Select(s => s.Name))}");

            return state;
        }

        public static PointAction From(short id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
                throw new OrderingDomainException($"Possible values for PointActions: {String.Join(",", List().Select(s => s.Name))}");

            return state;
        }
    }
}
