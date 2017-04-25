using Gestion.Common.Domain.Auth;
using Gestion.Common.Domain.Seguridad;
using System.Data.Entity;

namespace Gestion.Common.Data.Auth
{
    public interface ISecurityDataContext : IUnitOfWork
    {
        DbSet<Audience> Audiences { get; }
        DbSet<RefreshToken> RefreshTokens { get; }
        DbSet<Usuario> Usuarios { get; }
        DbSet<Membresia> Membresias { get; }
    }
}
