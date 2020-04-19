using Autofac;
using Order.Domain.AggregatesModel.ClientAggregate;
using Order.Domain.AggregatesModel.CourierAggregate;
using Order.Domain.AggregatesModel.DeliveryOrderAggregate;
using Order.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Order.API.Infrastructure.AutofacModules
{
    public class ApplicationModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ClientRepository>().As<IClientRepository>().InstancePerLifetimeScope();
            builder.RegisterType<CourierRepository>().As<ICourierRepository>().InstancePerLifetimeScope();
            builder.RegisterType<DeliveryOrderRepository>().As<IDeliveryOrderRepository>().InstancePerLifetimeScope();
        }
    }
}
