using System;
using System.Threading.Tasks;
using Delivery.Domain.AggregatesModel.DeliveryAggregate;
using Delivery.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Delivery.Infrastructure.Repositories
{
    public class DeliveryRepository : IDeliveryRepository
    {
        private readonly DeliveryContext _context;

        public DeliveryRepository(DeliveryContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }

        public IUnitOfWork UnitOfWork => _context;

        public Domain.AggregatesModel.DeliveryAggregate.Delivery Add(
            Domain.AggregatesModel.DeliveryAggregate.Delivery delivery)
        {
            return _context.Deliveries.Add(delivery).Entity;
        }

        public async Task<Domain.AggregatesModel.DeliveryAggregate.Delivery> GetAsync(Guid deliveryId)
        {
            var delivery = await _context.Deliveries.FirstOrDefaultAsync(x => x.Id == deliveryId);

            if (delivery != null)
                await _context.Entry(delivery)
                    .Reference(i => i.DeliveryStatus).LoadAsync();

            return delivery;
        }

        public void Update(Domain.AggregatesModel.DeliveryAggregate.Delivery delivery)
        {
            _context.Entry(delivery).State = EntityState.Modified;
        }

        public void Delete(Domain.AggregatesModel.DeliveryAggregate.Delivery delivery)
        {
            _context.Entry(delivery).State = EntityState.Deleted;
        }
    }
}