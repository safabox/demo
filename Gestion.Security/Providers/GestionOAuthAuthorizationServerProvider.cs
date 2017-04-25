using Gestion.Common.Domain.Auth;
using Gestion.Security.Factories;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Gestion.Security.Providers
{
    public class GestionOAuthAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private readonly IAudiencesStoreFactory audiencesStoreFactory;
        private readonly IUserManagerFactory userManagerFactory;

        public GestionOAuthAuthorizationServerProvider(IAudiencesStoreFactory audiencesStoreFactory, IUserManagerFactory userManagerFactory)
        {
            this.audiencesStoreFactory = audiencesStoreFactory;
            this.userManagerFactory = userManagerFactory;
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId = string.Empty;
            string clientSecret = string.Empty;
            string symmetricKeyAsBase64 = string.Empty;

            Helper.AddCORSHeades(context.OwinContext);

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (context.ClientId != null)
            {
                using (var audiencesStore = this.audiencesStoreFactory.Create())
                {
                    var audience = await audiencesStore.FindAsync(context.ClientId);

                    if (audience != null)
                    {
                        if (audience.Active)
                        {
                            // No se valida clientSecret ya que en las apps Javascript no se pueden mantenter claves, por lo que no tiene sentido.
                            // En caso de añadir otro tipo de cliente, es recomendable validar clientId/clientSecret


                            context.OwinContext.Set<string>("as:clientAllowedOrigin", audience.AllowedOrigin);
                            context.OwinContext.Set<string>("as:clientRefreshTokenLifeTime", audience.RefreshTokenLifeTime.ToString());

                            context.Validated();
                        }
                        else
                        {
                            context.SetError("invalid_clientId", "Client inactivo");
                        }
                    }
                    else
                    {
                        context.SetError("invalid_clientId", string.Format("El client_id '{0}' es inválido", context.ClientId));
                    }
                }
            }
            else
            {
                context.SetError("invalid_clientId", "No se especificó client_id");
            }
        }

        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            bool validUser = false;

            Helper.AddCORSHeades(context.OwinContext);

            using (var userManager = this.userManagerFactory.Create())
            {

                var user = userManager.GetByUserName(context.UserName);
                if (user != null)
                {
                    if (user.ValidatePassword(context.Password))
                    {
                        var identity = new ClaimsIdentity("JWT");
                        identity.AddClaim(new Claim("user_sid", user.Sid));
                        identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
                        identity.AddClaim(new Claim("display_name", user.DisplayName));
                        identity.AddClaim(new Claim("sub", user.UserName));
                        identity.AddClaim(new Claim("password_expired", user.PasswordExpired ? "true" : "false"));

                        var permissions = userManager.GetPermissionsForUser(context.UserName);

                        var claims = GetClaims(permissions);

                        foreach (var claim in claims)
                        {
                            identity.AddClaim(new Claim(ClaimTypes.Role, claim));
                        }

                        var props = new AuthenticationProperties(new Dictionary<string, string>
                        {
                            {
                                 "audience", (context.ClientId == null) ? string.Empty : context.ClientId
                            }
                        });

                        var ticket = new AuthenticationTicket(identity, props);
                        context.Validated(ticket);
                        validUser = true;
                    }
                }
                if (!validUser)
                {
                    context.SetError("invalid_grant", "Usuario o clave inválida");
                }
            }
            return Task.FromResult<object>(null);
        }

        private IEnumerable<string> GetClaims(IEnumerable<IPermission> permissions)
        {
            var permByResource = permissions.GroupBy(x => x.Resource);

            var list = permByResource.Select(x => String.Format("{0}:{1}", x.Key, String.Join(",", x.Select(s => s.Action))));
            return list;
        }

    }
}
