using Gestion.Services;
using Gestion.Data;
using Autofac;
using Autofac.Integration.WebApi;
using Owin;
using System.Reflection;
using System.Web.Http;

namespace Gestion.API.Config
{
    public class DIConfig
    {
        internal static void Configure(IAppBuilder app, HttpConfiguration config)
        {
            var builder = CreateContainerBuilder();

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(config);
        }

        private static ContainerBuilder CreateContainerBuilder()
        {
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterAssemblyModules(Assembly.GetExecutingAssembly());
            builder.RegisterModule(new ServicesDIModule());
            builder.RegisterModule(new DataDIModule());
            return builder;
        }
    }
}