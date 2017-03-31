using Autofac;
using AutoMapper;

namespace Gestion.API.Config.DIModules
{
    public class AutoMapperDIModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(context => CreateMapperConfiguration(context))
                .AsSelf()
                .SingleInstance();

            builder.Register(context => context.Resolve<MapperConfiguration>()
                .CreateMapper(context.Resolve))
                .As<IMapper>()
                .InstancePerLifetimeScope();
        }

        private MapperConfiguration CreateMapperConfiguration(IComponentContext context)
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfiles(System.Reflection.Assembly.GetExecutingAssembly());
            });
        }
    }
}