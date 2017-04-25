using Microsoft.Owin.Security.OAuth;

namespace Gestion.Security.Factories
{
    public interface IOAuthAuthorizationServerOptionsFactory
    {
        OAuthAuthorizationServerOptions GetOptions();
    }
}
