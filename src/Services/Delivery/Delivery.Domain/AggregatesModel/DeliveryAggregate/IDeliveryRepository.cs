using Delivery.Domain.Common;
using System;
using System.Threading.Tasks;

namespace Delivery.Domain.AggregatesModel.DeliveryAggregate
{
    //This is just the RepositoryContracts or Interface defined at the Domain Layer
    //as requisite for the Delivery Aggregate
    public interface IDeliveryRepository : IRepository<Delivery>
    {
        Delivery Add(Delivery delivery);

        void Update(Delivery delivery);

        Task<Delivery> GetAsync(Guid deliveryId);

        void Delete(Delivery delivery);
    }
}
