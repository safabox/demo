using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestion.Common.Domain.Seguridad
{
    public class Usuario : EntidadModificable
    {
        #region Datos Personales

        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }

        #endregion

        #region Seguridad

        public string NombreUsuario { get; set; }

        public string Password { get; set; }
        public bool ForzarCambioPassword { get; set; }

        #endregion

        #region Membresías

        public virtual ICollection<Membresia> Membresias { get; set; }

        #endregion

        #region Modificacion/Estado

        public bool Activo { get; set; }

        #endregion

        public Usuario()
        {
            this.Membresias = new List<Membresia>();
            this.Activo = true;
            this.FechaCreacion = DateTime.Now;
        }
    }
}
