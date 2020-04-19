using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Domain.AggregatesModel.CourierAggregate;

namespace Order.Infrastructure.EntityConfigurations
{
    class CourierConfiguration : IEntityTypeConfiguration<Courier>
    {
        public void Configure(EntityTypeBuilder<Courier> courierConfiguration)
        {
            courierConfiguration.ToTable("couriers", DeliveryOrderContext.DEFAULT_SCHEMA);

            courierConfiguration.HasKey(x => x.Id);
            courierConfiguration.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            courierConfiguration.Ignore(x => x.DomainEvents);

            courierConfiguration.Property(x => x.FirstName)
                .IsRequired();
            courierConfiguration.Property(x => x.LastName);
            courierConfiguration.Property(x => x.Phone)
                .IsRequired();

            courierConfiguration.Property(x => x.IdentityGuid)
                .IsRequired();
            courierConfiguration.HasIndex(nameof(Courier.IdentityGuid))
                .IsUnique();
        }
    }
}
