using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Domain.AggregatesModel.DeliveryOrderAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Infrastructure.EntityConfigurations
{
    class DeliveryLocationActionConfiguration : IEntityTypeConfiguration<DeliveryLocationAction>
    {
        public void Configure(EntityTypeBuilder<DeliveryLocationAction> builder)
        {
            builder.ToTable("deliveryLocationActions", DeliveryOrderContext.DEFAULT_SCHEMA);

            builder.HasKey(ct => ct.Id);

            builder.Property(ct => ct.Id)
                .HasDefaultValue(1)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(ct => ct.Name)
                .HasMaxLength(200)
                .IsRequired();
        }
    }
}
