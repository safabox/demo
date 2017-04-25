using System.Security.Claims;
using System.Threading.Tasks;

namespace Gestion.Security
{
    public interface ISecurityContext
    {
        ClaimsPrincipal GetPrincipal();

        bool CheckAccess(string action, params string[] resources);
        Task<bool> CheckAccessAsync(string action, params string[] resources);
    }
}
