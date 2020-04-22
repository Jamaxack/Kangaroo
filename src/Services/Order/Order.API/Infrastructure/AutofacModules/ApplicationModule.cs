using Autofac;
using Order.API.Application.Queries;
using Order.Domain.AggregatesModel.ClientAggregate;
using Order.Domain.AggregatesModel.CourierAggregate;
using Order.Domain.AggregatesModel.DeliveryOrderAggregate;
using Order.Infrastructure.Repositories;

namespace Order.API.Infrastructure.AutofacModules
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

            builder.RegisterType<CourierRepository>()
                .As<ICourierRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<DeliveryOrderRepository>()
                .As<IDeliveryOrderRepository>()
                .InstancePerLifetimeScope();
        }
    }
}
