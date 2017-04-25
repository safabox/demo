using Gestion.Services.Seguridad;
using Gestion.Common.Services.Seguridad;
using Autofac;

namespace Gestion.Services
{
    public class ServicesDIModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PermisoService>().As<IPermisoService>();
            builder.RegisterType<RolService>().As<IRolService>();
            builder.RegisterType<UsuarioService>().As<IUsuarioService>();
        }
    }
}
