using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Domain.AggregatesModel.ClientAggregate;

namespace Order.Infrastructure.EntityConfigurations
{
    class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> clientConfiguration)
        {
            clientConfiguration.ToTable("clients", DeliveryOrderContext.DEFAULT_SCHEMA);

            clientConfiguration.HasKey(x => x.Id);
            clientConfiguration.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            clientConfiguration.Ignore(x => x.DomainEvents);

            clientConfiguration.Property(x => x.FirstName)
                .IsRequired();
            clientConfiguration.Property(x => x.LastName);
            clientConfiguration.Property(x => x.Phone)
                .IsRequired();

            clientConfiguration.Property(x => x.IdentityGuid)
                .IsRequired();
            clientConfiguration.HasIndex(nameof(Client.IdentityGuid))
                .IsUnique();
        }
    }
}
