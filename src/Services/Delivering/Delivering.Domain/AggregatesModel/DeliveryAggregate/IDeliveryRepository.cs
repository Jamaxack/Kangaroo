using Delivering.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Delivering.Domain.AggregatesModel.DeliveryAggregate
{
    //This is just the RepositoryContracts or Interface defined at the Domain Layer
    //as requisite for the Delivery Aggregate
    public interface IDeliveryRepository : IRepository<Delivery>
    {
        Delivery Add(Delivery Delivery);

        void Update(Delivery Delivery);

        Task<Delivery> GetAsync(Guid DeliveryId);

        void Delete(Delivery Delivery);
    }
}
