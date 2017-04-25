using Gestion.Security.Factories;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Configuration;
using System.Web.Http;

namespace Gestion.API.Config
{
    public class AuthConfig
    {
        internal static void ConfigureAuthServer(IAppBuilder app, HttpConfiguration config)
        {
            var optionsFactory = (IOAuthAuthorizationServerOptionsFactory)config.DependencyResolver.GetService(typeof(IOAuthAuthorizationServerOptionsFactory));
            var options = optionsFactory.GetOptions();

            app.UseOAuthAuthorizationServer(options);
        }

        internal static void ConfigureResourceServer(IAppBuilder app)
        {
            var issuer = ConfigurationManager.AppSettings[Constants.AppSettings.AUTH_SERVER_TOKEN_ISSUER];
            var audience = "683DD6FEC91749DAA00B103BA026215C";

            var secret = Microsoft.Owin.Security.DataHandler.Encoder.TextEncodings.Base64Url.Decode("PKJHc8JIsdDICF+kzdfTui3BYaHyiFY10oxH1JVEsDQ=");

            var options = new JwtBearerAuthenticationOptions
            {
                AuthenticationMode = AuthenticationMode.Active,
                AllowedAudiences = new[] { audience },
                IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
                    {
                        new SymmetricKeyIssuerSecurityTokenProvider(issuer, secret)
                    },
                Provider = new OAuthBearerAuthenticationProvider()
            };

            app.UseJwtBearerAuthentication(options);
            app.UseResourceAuthorization(new Gestion.Security.Authorization.GestionResourceAuthorizationManager());
        }
    }
}