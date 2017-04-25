
namespace Gestion.Common.Domain.Auth
{
    public interface IPermission
    {
        string Resource { get; }
        string Action { get; }
    }
}
