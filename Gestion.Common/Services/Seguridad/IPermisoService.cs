using Gestion.Common.Domain.Seguridad;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion.Common.Services.Seguridad
{
    public interface IPermisoService
    {
        IQueryable<Permiso> GetAll();

        Task<Permiso> GetAsync(long id);
    }
}
