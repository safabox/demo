using Gestion.Common.Domain;
using Gestion.Common.Utils;
using Gestion.API.Models.Membresias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gestion.API.Models.Usuarios
{
    public class UsuarioModel : Model
    {
        public long Id { get; set; }

        #region Datos Personales

        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }

        #endregion

        #region Seguridad

        public string NombreUsuario { get; set; }


        #endregion

        #region Membresías
        public IEnumerable<MembresiaModel> Membresias { get; set; }
        #endregion

        public EstadosEntidad Estado { get; set; }
        public string EstadoDescripcion
        {
            get
            {
                return this.Estado.GetDescription();
            }
        }
    }
}