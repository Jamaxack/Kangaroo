using Microsoft.EntityFrameworkCore;
using Order.Domain.AggregatesModel.DeliveryOrderAggregate;
using Order.Domain.Common;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Order.Infrastructure.Repositories
{
    public class DeliveryOrderRepository : IDeliveryOrderRepository
    {
        readonly DeliveryOrderContext _context;
        public IUnitOfWork UnitOfWork => _context;
        public DeliveryOrderRepository(DeliveryOrderContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }

        public DeliveryOrder Add(DeliveryOrder order)
        {
            return _context.DeliveryOrders.Add(order).Entity;
        }

        public async Task<DeliveryOrder> GetAsync(Guid orderId)
        {
            var order = await _context.DeliveryOrders
                .Include(x => x.DeliveryLocations)
                .FirstOrDefaultAsync(x => x.Id == orderId); 

            if (order != null)
            {
                await _context.Entry(order)
                    .Collection(i => i.DeliveryLocations).LoadAsync();
                await _context.Entry(order)
                    .Reference(i => i.DeliveryOrderStatus).LoadAsync();
            }

            return order;
        }

        public void Update(DeliveryOrder order)
        {
            _context.Entry(order).State = EntityState.Modified;
        } 
    }
}
