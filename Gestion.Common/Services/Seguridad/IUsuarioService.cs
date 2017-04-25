using Gestion.Common.Domain.Seguridad;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion.Common.Services.Seguridad
{
    public interface IUsuarioService
    {
        Task<Usuario> GetAsync(long id);
        
        IQueryable<Usuario> GetAll(bool includeDeleted = false);

        OperationResult<Usuario> Add(Usuario entity);

        Task<OperationResult<Usuario>> Update(long id, Usuario entity);

        Task<OperationResult<int>> SaveChangesAsync(string comments = null);

        Task<OperationResult<Usuario>> Delete(long id);
        Task<OperationResult<Usuario>> Recover(long id);
    }
}
