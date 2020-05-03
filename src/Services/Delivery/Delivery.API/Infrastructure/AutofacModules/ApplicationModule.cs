using Autofac;
using Delivery.API.Application.Queries;
using Delivery.Domain.AggregatesModel.ClientAggregate;
using Delivery.Domain.AggregatesModel.DeliveryOrderAggregate;
using Delivery.Infrastructure.Repositories;

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
            builder.Register(x => new DeliveryOrderQueries(QueriesConnectionString))
                .As<IDeliveryOrderQueries>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ClientRepository>()
                .As<IClientRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<DeliveryOrderRepository>()
                .As<IDeliveryOrderRepository>()
                .InstancePerLifetimeScope();
        }
    }
}
