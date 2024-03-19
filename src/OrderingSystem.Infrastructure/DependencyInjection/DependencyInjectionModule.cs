using System.Reflection;
using Autofac;
using CloudyWing.OrderingSystem.Infrastructure.Util;
using Module = Autofac.Module;

namespace CloudyWing.OrderingSystem.Infrastructure.DependencyInjection {
    public class DependencyInjectionModule : Module {
        private readonly Assembly[] assemblies;

        public DependencyInjectionModule(params Assembly[] assemblies) {
            ExceptionUtils.ThrowIfNull(() => assemblies);

            this.assemblies = assemblies;
        }

        protected override void Load(ContainerBuilder builder) {
            builder.RegisterAssemblyTypes(assemblies)
                .Where(x => x.IsAssignableTo(typeof(ITransientDependency)))
                .AsImplementedInterfaces()
                .AsSelf()
                .InstancePerDependency();

            builder.RegisterAssemblyTypes(assemblies)
                .Where(x => x.IsAssignableTo(typeof(IScopedDependency)))
                .AsImplementedInterfaces()
                .AsSelf()
                .InstancePerLifetimeScope();
        }
    }
}
