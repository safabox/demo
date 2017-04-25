using Microsoft.Owin.Security.OAuth;
using System.Threading.Tasks;

namespace Gestion.Security.Providers
{
    public class GestionOAuthBearerAuthenticationProvider : OAuthBearerAuthenticationProvider
    {
        public override Task RequestToken(OAuthRequestTokenContext context)
        {
            var token = context.Request.Headers.Get("Authorization");
            if (!string.IsNullOrEmpty(token))
            {
                token = token.Substring("Bearer ".Length);
            }
            if (string.IsNullOrEmpty(token))
            {
                token = context.Request.Query.Get("access_token");
            }
            context.Token = token;

            return Task.FromResult<object>(null);
        }
    }
}
