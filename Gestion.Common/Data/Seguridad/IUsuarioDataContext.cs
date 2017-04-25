using Gestion.Common.Domain.Seguridad;
using System.Data.Entity;


namespace Gestion.Common.Data.Seguridad
{
    public interface IUsuarioDataContext : IEntidadModificableDataContext
    {
        DbSet<Usuario> Usuarios { get; }
        DbSet<Rol> Roles { get; }
    }
}
