using Delivery.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Delivery.Domain.AggregatesModel.DeliveryOrderAggregate
{
    //This is just the RepositoryContracts or Interface defined at the Domain Layer
    //as requisite for the DeliveryOrder Aggregate
    public interface IDeliveryOrderRepository : IRepository<DeliveryOrder>
    {
        DeliveryOrder Add(DeliveryOrder deliveryOrder);

        void Update(DeliveryOrder deliveryOrder);

        Task<DeliveryOrder> GetAsync(Guid deliveryOrderId);

        void Delete(DeliveryOrder deliveryOrder);
    }
}
