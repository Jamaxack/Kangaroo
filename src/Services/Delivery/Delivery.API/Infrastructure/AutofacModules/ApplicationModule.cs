using Autofac;
using Delivery.API.Application.Queries;
using Delivery.Domain.AggregatesModel.ClientAggregate;
using Delivery.Domain.AggregatesModel.DeliveryAggregate;
using Delivery.Infrastructure.Repositories;
using Kangaroo.BuildingBlocks.EventBus.Abstractions;
using System.Reflection;

namespace Delivery.API.Infrastructure.AutofacModules
{
    public class ApplicationModule : Autofac.Module
    {
        public string QueriesConnectionString { get; }

        public ApplicationModule(string queriesConnectionString)
        {
            QueriesConnectionString = queriesConnectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(x => new DeliveryQueries(QueriesConnectionString))
                .As<IDeliveryQueries>()
                .InstancePerLifetimeScope();

            builder.Register(x => new ClientQueries(QueriesConnectionString))
                .As<IClientQueries>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ClientRepository>()
                .As<IClientRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<DeliveryRepository>()
                .As<IDeliveryRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(Startup).GetTypeInfo().Assembly)
               .AsClosedTypesOf(typeof(IIntegrationEventHandler<>));
        }
    }
}
