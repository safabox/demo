using Gestion.Common.Domain;
using Gestion.Common.Utils;

namespace Gestion.API.Models.Usuarios
{
    public class UsuarioListModel : Model
    {
        public long Id { get; set; }
        public string NombreUsuario { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
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