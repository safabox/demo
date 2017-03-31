using Autofac;

namespace Gestion.Data
{
    public class DataDIModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<GestionDbContext>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
