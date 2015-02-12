using System;
using System.Linq;
using Autofac;

namespace Core
{
    public class DependencyBuilder
    {
        public static ContainerBuilder SetupDependencyInjection()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                .Where(t => t.GetInterfaces()
                .Any(i => i.IsAssignableFrom(typeof(IDependency))))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            return builder;
        }
    }
}
