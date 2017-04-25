using Gestion.Common.Utils;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Thinktecture.IdentityModel;
using Thinktecture.IdentityModel.Owin.ResourceAuthorization;

namespace Gestion.Security
{
    public class SecurityContext : ISecurityContext
    {
        private readonly IResourceAuthorizationManager authorizationManager;

        public SecurityContext(IResourceAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }

        public ClaimsPrincipal GetPrincipal()
        {
            var principal = Thread.CurrentPrincipal as ClaimsPrincipal;

            return principal ?? Principal.Anonymous;
        }

        public bool CheckAccess(string action, params string[] resources)
        {
            return AsyncHelper.RunSync<bool>(() => CheckAccessAsync(action, resources));
        }

        public Task<bool> CheckAccessAsync(string action, params string[] resources)
        {
            var context = GetContext(action, resources);

            return this.authorizationManager.CheckAccessAsync(context);
        }

        private ResourceAuthorizationContext GetContext(string action, params string[] resources)
        {
            return new ResourceAuthorizationContext(GetPrincipal(), action, resources);
        }
    }
}
