using FrameLog;
using System.Collections.Generic;

namespace Gestion.Common.Domain.Seguridad
{
    public class Recurso : IHasLoggingReference
    {
        public string Codigo { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<Permiso> Permisos { get; set; }

        public object Reference
        {
            get
            {
                return this.Codigo;
            }
        }
    }
}
