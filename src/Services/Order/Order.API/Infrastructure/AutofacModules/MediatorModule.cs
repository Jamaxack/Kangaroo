using Autofac;
using MediatR;
using Order.API.Application.DomainEventHandlers.NewDeliveryOrderCreatedEvent;
using System.Reflection;

namespace Order.API.Infrastructure.AutofacModules
{
    public class MediatorModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register the DomainEventHandler classes (they implement INotificationHandler<>) in assembly holding the Domain Events
            builder.RegisterAssemblyTypes(typeof(NewDeliveryOrderCreatedDomainEventHandler).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(INotificationHandler<>));
        }
    }
}
