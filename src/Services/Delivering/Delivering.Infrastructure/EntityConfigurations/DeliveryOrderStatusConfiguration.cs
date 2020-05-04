using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Delivering.Domain.AggregatesModel.DeliveryOrderAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace Delivering.Infrastructure.EntityConfigurations
{
    class DeliveryOrderStatusConfiguration : IEntityTypeConfiguration<DeliveryOrderStatus>
    {
        public void Configure(EntityTypeBuilder<DeliveryOrderStatus> builder)
        {
            builder.ToTable("DeliveryOrderStatus", DeliveringContext.DEFAULT_SCHEMA);

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
