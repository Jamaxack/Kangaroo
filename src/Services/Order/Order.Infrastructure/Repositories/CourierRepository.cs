using Microsoft.EntityFrameworkCore;
using Order.Domain.AggregatesModel.CourierAggregate;
using Order.Domain.Common;
using System;
using System.Threading.Tasks;

namespace Order.Infrastructure.Repositories
{
    public class CourierRepository : ICourierRepository
    {
        readonly DeliveryOrderContext _context;
        public IUnitOfWork UnitOfWork => _context;
        public CourierRepository(DeliveryOrderContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }

        public Courier Add(Courier courier)
        {
            if (courier.IsTransient())
            {
                return _context.Couriers
                    .Add(courier)
                    .Entity;
            }
            else
            {
                return courier;
            }
        }

        public Courier Update(Courier courier)
        {
            return _context.Couriers
                .Update(courier)
                .Entity;
        }

        public async Task<Courier> FindAsync(Guid courierIdentityGuid)
        {
            var courier = await _context.Couriers
                .SingleOrDefaultAsync(b => b.IdentityGuid == courierIdentityGuid);

            return courier;
        }

        public async Task<Courier> FindByIdAsync(Guid id)
        {
            var courier = await _context.Couriers
                .SingleOrDefaultAsync(b => b.Id == id);

            return courier;
        }
    }
}
