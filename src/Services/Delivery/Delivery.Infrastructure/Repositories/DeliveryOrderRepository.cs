using Microsoft.EntityFrameworkCore;
using Delivery.Domain.AggregatesModel.DeliveryOrderAggregate;
using Delivery.Domain.Common;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery.Infrastructure.Repositories
{
    public class DeliveryOrderRepository : IDeliveryOrderRepository
    {
        readonly DeliveryContext _context;
        public IUnitOfWork UnitOfWork => _context;
        public DeliveryOrderRepository(DeliveryContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }

        public DeliveryOrder Add(DeliveryOrder deliveryOrder)
        {
            return _context.DeliveryOrders.Add(deliveryOrder).Entity;
        }

        public async Task<DeliveryOrder> GetAsync(Guid deliveryOrderId)
        {
            var deliveryOrder = await _context.DeliveryOrders.FirstOrDefaultAsync(x => x.Id == deliveryOrderId);

            if (deliveryOrder != null)
            {
                await _context.Entry(deliveryOrder)
                    .Reference(i => i.DeliveryOrderStatus).LoadAsync();
            }

            return deliveryOrder;
        }

        public void Update(DeliveryOrder deliveryOrder)
        {
            _context.Entry(deliveryOrder).State = EntityState.Modified;
        }

        public void Delete(DeliveryOrder deliveryOrder)
        {
            _context.Entry(deliveryOrder).State = EntityState.Deleted;
        }
    }
}
