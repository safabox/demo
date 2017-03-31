using Gestion.Common.Domain.Seguridad;
using System.Data.Entity;

namespace Gestion.Common.Data.Seguridad
{
    public interface IPermisoDataContext
    {
        DbSet<Permiso> Permisos { get; }
    }
}
