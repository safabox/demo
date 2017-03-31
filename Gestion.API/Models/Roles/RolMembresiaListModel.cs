using System;

namespace Gestion.API.Models.Roles
{
    public class RolMembresiaListModel
    {
        public string UsuarioNombreUsuario { get; set; }
        public string UsuarioApellido { get; set; }
        public string UsuarioNombre { get; set; }
        public DateTime VigenteDesde { get; set; }
        public Nullable<DateTime> VigenteHasta { get; set; }
    }
}