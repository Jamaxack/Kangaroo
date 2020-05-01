using Microsoft.EntityFrameworkCore;
using DeliveryOrder.Domain.AggregatesModel.DeliveryOrderAggregate;
using DeliveryOrder.Domain.Common;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryOrder.Infrastructure.Repositories
{
    public class DeliveryOrderRepository : IDeliveryOrderRepository
    {
        readonly DeliveryOrderContext _context;
        public IUnitOfWork UnitOfWork => _context;
        public DeliveryOrderRepository(DeliveryOrderContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }

        public Domain.AggregatesModel.DeliveryOrderAggregate.DeliveryOrder Add(Domain.AggregatesModel.DeliveryOrderAggregate.DeliveryOrder deliveryOrder)
        {
            return _context.DeliveryOrders.Add(deliveryOrder).Entity;
        }

        public async Task<Domain.AggregatesModel.DeliveryOrderAggregate.DeliveryOrder> GetAsync(Guid deliveryOrderId)
        {
            var deliveryOrder = await _context.DeliveryOrders
                .Include(x => x.DeliveryLocations)
                .FirstOrDefaultAsync(x => x.Id == deliveryOrderId);

            if (deliveryOrder != null)
            {
                await _context.Entry(deliveryOrder)
                    .Collection(i => i.DeliveryLocations).LoadAsync();
                await _context.Entry(deliveryOrder)
                    .Reference(i => i.DeliveryOrderStatus).LoadAsync();
            }

            return deliveryOrder;
        }

        public void Update(Domain.AggregatesModel.DeliveryOrderAggregate.DeliveryOrder deliveryOrder)
        {
            _context.Entry(deliveryOrder).State = EntityState.Modified;
        }
    }
}
