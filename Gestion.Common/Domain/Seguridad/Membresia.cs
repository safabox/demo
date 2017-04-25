using System;
using System.Linq.Expressions;

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
            this.VigenteDesde = DateTime.UtcNow.Date;
        }

        public object Reference
        {
            get
            {
                return new { this.RolId, this.UsuarioId };
            }
        }

        #region Predicates

        public static Expression<Func<Membresia, Boolean>> ActivasPredicate(DateTime? vigencia = null)
        {
            var fecha = vigencia.GetValueOrDefault(DateTime.UtcNow);

            return x => x.VigenteDesde <= fecha
                && (x.VigenteHasta == null || x.VigenteHasta.Value >= fecha)
                && (x.Usuario.Estado == EstadosEntidad.Activa)
                && (x.Rol.Estado == EstadosEntidad.Activa);
        }

        #endregion
    }
}
