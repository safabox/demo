using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Thinktecture.IdentityModel.Owin.ResourceAuthorization;

namespace Gestion.Security.Authorization
{
    public class GestionResourceAuthorizationManager : ResourceAuthorizationManager
    {
        public override Task<bool> CheckAccessAsync(ResourceAuthorizationContext context)
        {
            var resourceName = GetResourceName(context);

            if (resourceName == Resources.Roles)
            {
                return CheckRolesAccessAsync(context);
            }

            if (resourceName == Resources.Usuarios)
            {
                return CheckUsuariosAccessAsync(context);
            }
            return CheckDefaultAccessAsync(resourceName, context);
        }

        private Task<bool> CheckUsuariosAccessAsync(ResourceAuthorizationContext context)
        {
            var principal = context.Principal;
            var resourceName = Resources.Usuarios;

            if (principal.Identity.IsAuthenticated)
            {
                var actionName = GetActionName(context);

                if (HasResourceAndAction(principal, resourceName, actionName))
                {
                    // No se puede editar o eliminar a si mismo
                    if (actionName == Resources.UsuariosActions.Editar || actionName == Resources.UsuariosActions.Eliminar)
                    {
                        var userId = principal.FindFirst("user_sid").Value;
                        if (GetResourceId(context) == userId)
                        {
                            return Nok();
                        }
                    }

                    return Ok();
                }
            }
            return Nok();
        }

        private Task<bool> CheckRolesAccessAsync(ResourceAuthorizationContext context)
        {
            var principal = context.Principal;
            var resourceName = Resources.Roles;

            if (principal.Identity.IsAuthenticated)
            {
                var actionName = GetActionName(context);

                if (HasResourceAndAction(principal, resourceName, actionName))
                {
                    // No se puede eliminar el Rol 1
                    if (actionName == Resources.RolesActions.Eliminar)
                    {
                        if (GetResourceId(context) == "1")
                        {
                            return Nok();
                        }
                    }

                    return Ok();
                }
            }
            return Nok();
        }

        private Task<bool> CheckDefaultAccessAsync(string resourceName, ResourceAuthorizationContext context)
        {
            var principal = context.Principal;

            if (principal.Identity.IsAuthenticated)
            {
                var actionName = GetActionName(context);

                if (HasResourceAndAction(principal, resourceName, actionName))
                {
                    return Ok();
                }
            }
            return Nok();
        }

        private bool HasResourceAndAction(ClaimsPrincipal principal, string resource, string action)
        {
            var pattern = string.Format("{0}:(.+,)*{1}(,.*)*$", resource, action);
            return principal.Claims.Any(x => x.Type == ClaimTypes.Role && Regex.Match(x.Value, pattern, RegexOptions.Singleline).Success);
        }

        private string GetResourceName(ResourceAuthorizationContext context)
        {
            return GetClaimValue(context.Resource, "name");
        }

        private string GetActionName(ResourceAuthorizationContext context)
        {
            return GetClaimValue(context.Action, "name");
        }

        private string GetResourceId(ResourceAuthorizationContext context)
        {
            return GetClaimValue(context.Resource, "id");
        }

        private string GetClaimValue(IEnumerable<Claim> claims, string type)
        {
            var claim = claims.FirstOrDefault(x => x.Type == type);
            if (claim != null)
            {
                return claim.Value;
            }
            return string.Empty;
        }
    }
}
