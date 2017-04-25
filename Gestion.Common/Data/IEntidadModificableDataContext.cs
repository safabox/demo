using Gestion.Common.Data.Audit;
using Gestion.Common.Domain.Seguridad;

namespace Gestion.Common.Data
{
    public interface IEntidadModificableDataContext : IUnitOfWork, IAuditDataContext
    {
        Usuario GetUsuarioByNombreUsuario(string nombreUsuario);
    }
}
