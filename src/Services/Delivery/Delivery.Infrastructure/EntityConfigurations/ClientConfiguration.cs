using Delivery.Domain.AggregatesModel.ClientAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Delivery.Infrastructure.EntityConfigurations
{
    internal class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> clientConfiguration)
        {
            clientConfiguration.ToTable("Clients", DeliveryContext.DefaultSchema);

            clientConfiguration.HasKey(x => x.Id);
            clientConfiguration.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            clientConfiguration.Ignore(x => x.DomainEvents);

            clientConfiguration.Property(x => x.FirstName)
                .IsRequired();
            clientConfiguration.Property(x => x.LastName);
            clientConfiguration.Property(x => x.Phone)
                .IsRequired();
        }
    }
}