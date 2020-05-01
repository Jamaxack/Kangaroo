using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DeliveryOrder.Domain.AggregatesModel.CourierAggregate;

namespace DeliveryOrder.Infrastructure.EntityConfigurations
{
    class CourierConfiguration : IEntityTypeConfiguration<Courier>
    {
        public void Configure(EntityTypeBuilder<Courier> courierConfiguration)
        {
            courierConfiguration.ToTable("Couriers", DeliveryOrderContext.DEFAULT_SCHEMA);

            courierConfiguration.HasKey(x => x.Id);
            courierConfiguration.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            courierConfiguration.Ignore(x => x.DomainEvents);

            courierConfiguration.Property(x => x.FirstName)
                .IsRequired();
            courierConfiguration.Property(x => x.LastName);
            courierConfiguration.Property(x => x.Phone)
                .IsRequired();
        }
    }
}
