using Gestion.Common.Domain.Seguridad;
using System.Data.Entity;

namespace Gestion.Common.Data.Seguridad
{
    public interface IRolDataContext : IEntidadModificableDataContext
    {
        DbSet<Rol> Roles { get; }
        DbSet<Permiso> Permisos { get; }
        DbSet<Membresia> Membresias { get; }
    }
}
