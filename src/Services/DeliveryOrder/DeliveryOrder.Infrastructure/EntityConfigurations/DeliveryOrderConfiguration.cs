using DeliveryOrder.Domain.AggregatesModel.ClientAggregate;
using DeliveryOrder.Domain.AggregatesModel.CourierAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace DeliveryOrder.Infrastructure.EntityConfigurations
{
    class DeliveryOrderConfiguration : IEntityTypeConfiguration<Domain.AggregatesModel.DeliveryOrderAggregate.DeliveryOrder>
    {
        public void Configure(EntityTypeBuilder<Domain.AggregatesModel.DeliveryOrderAggregate.DeliveryOrder> builder)
        {
            builder.ToTable("DeliveryOrders", DeliveryOrderContext.DEFAULT_SCHEMA);

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
            builder.Property(x => x.Price).IsRequired();
            builder.Property(x => x.Weight).IsRequired();
            builder.Property(x => x.Note).IsRequired(false);

            builder.HasOne(o => o.DeliveryOrderStatus)
                .WithMany()
                .HasForeignKey("_deliveryOrderStatusId");

            //PickUpLocation value object persisted as owned entity type supported since EF Core 2.0
            builder.OwnsOne(x => x.PickUpLocation, pickUpLocationSettings =>
            {
                pickUpLocationSettings.WithOwner();
                //ContactPerson value object persisted as owned entity type
                pickUpLocationSettings.OwnsOne(x => x.ContactPerson, contactPersonSettings =>
                {
                    contactPersonSettings.WithOwner();
                });
            });
            builder.OwnsOne(x => x.DropOffLocation, dropOffLocationSettings =>
            {
                dropOffLocationSettings.WithOwner();
                //ContactPerson value object persisted as owned entity type
                dropOffLocationSettings.OwnsOne(x => x.ContactPerson, contactPersonSettings =>
                {
                    contactPersonSettings.WithOwner();
                });
            });

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
