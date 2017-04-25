using Gestion.Common.Domain.Seguridad;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace Gestion.Common.Data
{
    public interface IUnitOfWork
    {
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        void SetEntityState<T>(T entry, EntityState state) where T : class;
        void Reload<T>(T entry) where T : class;

        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(Usuario user, string comments = null);
        Task<int> SaveChangesAsync(string userName, string comments = null);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken, Usuario user, string comments = null);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken, string userName, string comments = null);
        int SaveChanges(Usuario user, string comments = null);
    }
}
