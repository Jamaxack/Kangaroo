using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DeliveryOrder.Domain.AggregatesModel.DeliveryOrderAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryOrder.Infrastructure.EntityConfigurations
{
    class DeliveryLocationConfiguration : IEntityTypeConfiguration<DeliveryLocation>
    {
        public void Configure(EntityTypeBuilder<DeliveryLocation> builder)
        {
            builder.ToTable("DeliveryLocations", DeliveryOrderContext.DEFAULT_SCHEMA);

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Ignore(x => x.DomainEvents);

            builder.Property("_deliveryLocationActionId")
                 .UsePropertyAccessMode(PropertyAccessMode.Field)
                 .HasColumnName("DeliveryLocationActionId")
                 .IsRequired();
            builder.Property<Guid>("DeliveryOrderId").IsRequired();
            builder.Property(x => x.Address).IsRequired(false);
            builder.Property(x => x.BuildingNumber).IsRequired(false);
            builder.Property(x => x.EntranceNumber).IsRequired(false);
            builder.Property(x => x.FloorNumber).IsRequired(false);
            builder.Property(x => x.ApartmentNumber).IsRequired(false);
            builder.Property(x => x.Latitude);
            builder.Property(x => x.Longitude);
            builder.Property(x => x.Note).IsRequired(false);
            builder.Property(x => x.BuyoutAmount);
            builder.Property(x => x.TakingAmount).IsRequired();
            builder.Property(x => x.IsPaymentInThisDeliveryLocation).IsRequired();
            builder.Property(x => x.ArrivalStartDateTime).IsRequired(false);
            builder.Property(x => x.ArrivalFinishDateTime).IsRequired(false);
            builder.Property(x => x.CourierArrivedDateTime).IsRequired(false);

            //ContactPerson value object persisted as owned entity type supported since EF Core 2.0
            builder.OwnsOne(x => x.ContactPerson, settings => { settings.WithOwner(); });

            builder.HasOne(o => o.DeliveryLocationAction)
                .WithMany()
                .HasForeignKey("_deliveryLocationActionId");

        }
    }
}
