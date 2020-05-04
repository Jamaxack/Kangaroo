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

        public Delivery Add(Delivery Delivery)
        {
            return _context.Deliverys.Add(Delivery).Entity;
        }

        public async Task<Delivery> GetAsync(Guid DeliveryId)
        {
            var Delivery = await _context.Deliverys.FirstOrDefaultAsync(x => x.Id == DeliveryId);

            if (Delivery != null)
            {
                await _context.Entry(Delivery)
                    .Reference(i => i.DeliveryStatus).LoadAsync();
            }

            return Delivery;
        }

        public void Update(Delivery Delivery)
        {
            _context.Entry(Delivery).State = EntityState.Modified;
        }

        public void Delete(Delivery Delivery)
        {
            _context.Entry(Delivery).State = EntityState.Deleted;
        }
    }
}
