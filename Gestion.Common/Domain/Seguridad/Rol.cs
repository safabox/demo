using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestion.Common.Domain.Seguridad
{
    public class Rol : EntidadModificable
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<Membresia> Membresias { get; set; }

        public virtual ICollection<Permiso> Permisos { get; set; }

        public Rol()
        {
            this.Permisos = new List<Permiso>();
        }
    }
}
