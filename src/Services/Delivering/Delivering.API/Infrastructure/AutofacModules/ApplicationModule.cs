using Autofac;
using Delivering.API.Application.Queries;
using Delivering.Domain.AggregatesModel.ClientAggregate;
using Delivering.Domain.AggregatesModel.DeliveryOrderAggregate;
using Delivering.Infrastructure.Repositories;

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
