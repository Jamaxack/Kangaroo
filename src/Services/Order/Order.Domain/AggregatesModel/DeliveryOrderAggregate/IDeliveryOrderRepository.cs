using Order.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain.AggregatesModel.DeliveryOrderAggregate
{
    //This is just the RepositoryContracts or Interface defined at the Domain Layer
    //as requisite for the Order Aggregate
    public interface IDeliveryOrderRepository : IRepository<DeliveryOrder>
    {
        DeliveryOrder Add(DeliveryOrder order);

        void Update(DeliveryOrder order);

        Task<DeliveryOrder> GetAsync(Guid orderId);
    }
}
