using DeliveryOrder.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryOrder.Domain.AggregatesModel.CourierAggregate
{
    public interface ICourierRepository : IRepository<Courier>
    {
        Courier Add(Courier courier);
        Courier Update(Courier courier);
        Task<Courier> FindByIdAsync(Guid id);
    }
}
