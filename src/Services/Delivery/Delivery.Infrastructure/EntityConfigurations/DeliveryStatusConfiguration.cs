using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Delivery.Domain.AggregatesModel.DeliveryAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace Delivery.Infrastructure.EntityConfigurations
{
    class DeliveryStatusConfiguration : IEntityTypeConfiguration<DeliveryStatus>
    {
        public void Configure(EntityTypeBuilder<DeliveryStatus> builder)
        {
            builder.ToTable("DeliveryStatus", DeliveryContext.DEFAULT_SCHEMA);

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
