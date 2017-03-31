using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gestion.API.Models.Roles
{
    public class PostRolModel : Model
    {
        [Required(ErrorMessage = "{0} es obligatorio")]
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public IEnumerable<long> Permisos { get; set; }
    }
}