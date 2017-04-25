using Gestion.Data.Auth;
using Gestion.Security;
using Gestion.Security.PasswordPolicy;
using Gestion.Security.Authorization;
using Gestion.Security.Factories;
using Gestion.Security.Providers;
using Gestion.Security.Stores;
using Autofac;
using Microsoft.Owin.Security.OAuth;
using Thinktecture.IdentityModel.Owin.ResourceAuthorization;

namespace Gestion.API.Config.DIModules
{
    public class AuthDIModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<GestionOAuthAuthorizationServerOptionsFactory>().As<IOAuthAuthorizationServerOptionsFactory>();
            builder.RegisterType<GestionOAuthAuthorizationServerProvider>().As<IOAuthAuthorizationServerProvider>();
            builder.RegisterType<AudiencesStore>().As<IAudiencesStore>();
            builder.RegisterType<RefreshTokenStore>().As<IRefreshTokenStore>();
            builder.RegisterType<UserManager>().As<IUserManager>();
            builder.RegisterType<SecurityContext>().As<ISecurityContext>();
            builder.RegisterType<SimplePasswordPolicy>().As<IPasswordPolicy>();
            builder.RegisterType<GestionResourceAuthorizationManager>().As<IResourceAuthorizationManager>();

            builder.RegisterType<AuthComponentsFactory>()
                .As<IAudiencesStoreFactory>()
                .As<IRefreshTokenStoreFactory>()
                .As<IUserManagerFactory>();
        }

        class AuthComponentsFactory : IAudiencesStoreFactory, IRefreshTokenStoreFactory, IUserManagerFactory
        {
            private readonly IComponentContext context;

            public AuthComponentsFactory(IComponentContext context)
            {
                this.context = context;
            }

            IAudiencesStore IAudiencesStoreFactory.Create()
            {
                return Resolve<IAudiencesStore>();
            }

            IUserManager IUserManagerFactory.Create()
            {
                return Resolve<IUserManager>();
            }

            IRefreshTokenStore IRefreshTokenStoreFactory.Create()
            {
                return Resolve<IRefreshTokenStore>();
            }

            private T Resolve<T>()
            {
                return this.context.Resolve<T>();
            }
        }
    }
}