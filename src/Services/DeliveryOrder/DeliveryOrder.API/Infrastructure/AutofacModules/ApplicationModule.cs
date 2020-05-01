using Autofac;
using DeliveryOrder.API.Application.Queries;
using DeliveryOrder.Domain.AggregatesModel.ClientAggregate;
using DeliveryOrder.Domain.AggregatesModel.CourierAggregate;
using DeliveryOrder.Domain.AggregatesModel.DeliveryOrderAggregate;
using DeliveryOrder.Infrastructure.Repositories;

namespace DeliveryOrder.API.Infrastructure.AutofacModules
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
