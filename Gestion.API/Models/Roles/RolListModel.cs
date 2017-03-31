using Gestion.Common.Domain;
using Gestion.Common.Utils;

namespace Gestion.API.Models.Roles
{
    public class RolListModel : Model
    {
        public long Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

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