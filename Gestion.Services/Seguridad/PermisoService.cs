using Gestion.Common.Domain.Seguridad;
using Gestion.Common.Data.Seguridad;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion.Services.Seguridad
{
    public class PermisoService : IPermisoService
    {
        private readonly IPermisoDataContext context;

        public PermisoService(IPermisoDataContext context)
        {
            this.context = context;
        }

        public IQueryable<Permiso> GetAll()
        {
            return this.context.Permisos;
        }

        public Task<Permiso> GetAsync(long id)
        {
            return this.context.Permisos.FindAsync(id);
        }
    }
}
