using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gestion.API.Models.Permisos
{
    public class PermisoModel : Model
    {
        public long Id { get; set; }
        public string Descripcion { get; set; }
        public string RecursoDescripcion { get; set; }
    }
}