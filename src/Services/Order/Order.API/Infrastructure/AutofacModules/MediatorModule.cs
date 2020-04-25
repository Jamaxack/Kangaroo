using Autofac;
using MediatR;
using Order.API.Application.Commands;
using Order.API.Application.DomainEventHandlers.NewDeliveryOrderCreatedEvent;
using Order.Infrastructure.Idempotency;
using System.Reflection;

namespace Order.API.Infrastructure.AutofacModules
{
    public class MediatorModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
               .AsImplementedInterfaces();

            builder.RegisterType<RequestManager>().As<IRequestManager>();

            // Register all the Command classes (they implement IRequestHandler) in assembly holding the Commands
            builder.RegisterAssemblyTypes(typeof(CreateDeliveryOrderCommand).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>));

            // Register the DomainEventHandler classes (they implement INotificationHandler<>) in assembly holding the Domain Events
            builder.RegisterAssemblyTypes(typeof(NewDeliveryOrderCreatedDomainEventHandler)
                .GetTypeInfo().Assembly).AsClosedTypesOf(typeof(INotificationHandler<>));

            builder.Register<ServiceFactory>(context =>
            {
                var componentContext = context.Resolve<IComponentContext>();
                return type => { object obj; return componentContext.TryResolve(type, out obj) ? obj : null; };
            });
        }
    }
}
