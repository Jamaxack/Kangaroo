using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DeliveryOrder.Domain.AggregatesModel.DeliveryOrderAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryOrder.Infrastructure.EntityConfigurations
{
    class DeliveryLocationActionConfiguration : IEntityTypeConfiguration<DeliveryLocationAction>
    {
        public void Configure(EntityTypeBuilder<DeliveryLocationAction> builder)
        {
            builder.ToTable("DeliveryLocationActions", DeliveryOrderContext.DEFAULT_SCHEMA);

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
