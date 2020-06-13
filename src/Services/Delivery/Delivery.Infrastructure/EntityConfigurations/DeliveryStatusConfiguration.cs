using Delivery.Domain.AggregatesModel.DeliveryAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Delivery.Infrastructure.EntityConfigurations
{
    internal class DeliveryStatusConfiguration : IEntityTypeConfiguration<DeliveryStatus>
    {
        public void Configure(EntityTypeBuilder<DeliveryStatus> builder)
        {
            builder.ToTable("DeliveryStatus", DeliveryContext.DefaultSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasDefaultValue(1)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();
        }
    }
}