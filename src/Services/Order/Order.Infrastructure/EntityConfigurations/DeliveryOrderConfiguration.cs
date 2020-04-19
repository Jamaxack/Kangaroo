using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Domain.AggregatesModel.ClientAggregate;
using Order.Domain.AggregatesModel.CourierAggregate;
using Order.Domain.AggregatesModel.DeliveryOrderAggregate;
using System;

namespace Order.Infrastructure.EntityConfigurations
{
    class DeliveryOrderConfiguration : IEntityTypeConfiguration<DeliveryOrder>
    {
        public void Configure(EntityTypeBuilder<DeliveryOrder> builder)
        {
            builder.ToTable("deliveryOrders", DeliveryOrderContext.DEFAULT_SCHEMA);

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Ignore(x => x.DomainEvents);

            builder.Property<int>("_deliveryOrderStatusId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("DeliveryOrderStatusId")
                .IsRequired();
            builder.Property<Guid>("_clientId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("ClientId")
                .IsRequired();
            builder.Property<Guid?>("_courierId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("CourierId")
                .IsRequired(false);
            builder.Property(x => x.CreatedDateTime).IsRequired();
            builder.Property(x => x.FinishedDateTime).IsRequired(false);
            builder.Property(x => x.PaymentAmount).IsRequired();
            builder.Property(x => x.InsuranceAmount);
            builder.Property(x => x.Weight).IsRequired();
            builder.Property(x => x.Note).IsRequired(false);

            //DeliveryOrderNotificationSettings value object persisted as owned entity type supported since EF Core 2.0
            builder.OwnsOne(x => x.DeliveryOrderNotificationSettings, settings => { settings.WithOwner(); });

            // DDD Patterns comment:
            //Set as field (New since EF 1.1) to access the DeliveryLocation collection property through its field
            builder.Metadata
                .FindNavigation(nameof(DeliveryOrder.DeliveryLocations))
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasOne(o => o.DeliveryOrderStatus)
                .WithMany()
                .HasForeignKey("_deliveryOrderStatusId");
            builder.HasOne<Client>()
                .WithMany()
                .HasForeignKey("_clientId");
            builder.HasOne<Courier>()
                .WithMany()
                .IsRequired(false)
                .HasForeignKey("_courierId");

        }
    }
}
