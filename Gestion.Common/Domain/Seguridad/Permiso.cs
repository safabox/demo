using System.Collections.Generic;

namespace Gestion.Common.Domain.Seguridad
{
    public class Permiso : Entidad
    {
        public string RecursoCodigo { get; set; }

        public virtual Recurso Recurso { get; set; }

        public string Accion { get; set; }

        public string Descripcion { get; set; }

        public virtual ICollection<Rol> Roles { get; set; }

        #region Auth

        #endregion

    }
}
