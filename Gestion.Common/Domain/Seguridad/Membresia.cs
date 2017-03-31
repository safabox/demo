using System;

namespace Gestion.Common.Domain.Seguridad
{
    public class Membresia
    {
        public long UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }
        public long RolId { get; set; }
        public virtual Rol Rol { get; set; }
        public DateTime VigenteDesde { get; set; }
        public Nullable<DateTime> VigenteHasta { get; set; }

        public Membresia()
        {
            this.VigenteDesde = DateTime.Now.Date;
        }
    }
}
