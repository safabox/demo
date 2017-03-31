using Gestion.Common.Domain;
using Gestion.Common.Utils;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gestion.API.Models.Roles
{
    public class RolModel : Model
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "{0} es obligatorio")]
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public IEnumerable<long> Permisos { get; set; }

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