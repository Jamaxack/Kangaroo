using Order.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Domain.AggregatesModel.CourierAggregate
{
    public class Courier : Entity, IAggregateRoot
    {
        public Guid IdentityGuid { get; private set; }
    }
}
