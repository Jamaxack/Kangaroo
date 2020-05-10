using Microsoft.EntityFrameworkCore;
using Delivering.Domain.AggregatesModel.DeliveryAggregate;
using Delivering.Domain.Common;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Delivering.Infrastructure.Repositories
{
    public class DeliveryRepository : IDeliveryRepository
    {
        readonly DeliveringContext _context;
        public IUnitOfWork UnitOfWork => _context;
        public DeliveryRepository(DeliveringContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }

        public Delivery Add(Delivery delivery)
        {
            return _context.Deliveries.Add(delivery).Entity;
        }

        public async Task<Delivery> GetAsync(Guid deliveryId)
        {
            var delivery = await _context.Deliveries.FirstOrDefaultAsync(x => x.Id == deliveryId);

            if (delivery != null)
            {
                await _context.Entry(delivery)
                    .Reference(i => i.DeliveryStatus).LoadAsync();
            }

            return delivery;
        }

        public void Update(Delivery delivery)
        {
            _context.Entry(delivery).State = EntityState.Modified;
        }

        public void Delete(Delivery delivery)
        {
            _context.Entry(delivery).State = EntityState.Deleted;
        }
    }
}
