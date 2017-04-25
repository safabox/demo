using Gestion.Common;
using Gestion.Common.Domain.Seguridad;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion.Common.Services.Seguridad
{
    public interface IRolService
    {
        Task<Rol> GetAsync(long id);

        IQueryable<Rol> GetAll(bool includeDeleted = false);

        OperationResult<Rol> Add(Rol entity);

        Task<OperationResult<Rol>> Update(long id, Rol entity);

        Task<OperationResult<int>> SaveChangesAsync(string comments = null);

        Task<OperationResult<Rol>> Delete(long id);
        Task<OperationResult<Rol>> Recover(long id);
        IQueryable<Membresia> GetMembresiasActivas(long idRol);
    }
}
