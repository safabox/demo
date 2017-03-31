using Gestion.Common.Domain.Seguridad;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion.Services.Seguridad
{
    public interface IPermisoService
    {
        IQueryable<Permiso> GetAll();

        Task<Permiso> GetAsync(long id);
    }
}
