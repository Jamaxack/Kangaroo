using Autofac;
using Delivering.API.Application.Queries;
using Delivering.Domain.AggregatesModel.ClientAggregate;
using Delivering.Domain.AggregatesModel.DeliveryAggregate;
using Delivering.Infrastructure.Repositories;
using Kangaroo.BuildingBlocks.EventBus.Abstractions;
using System.Reflection;

namespace Delivering.API.Infrastructure.AutofacModules
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
