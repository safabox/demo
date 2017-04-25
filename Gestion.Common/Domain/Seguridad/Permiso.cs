using Gestion.Common.Domain.Auth;
using System.Collections.Generic;

namespace Gestion.Common.Domain.Seguridad
{
    public class Permiso : Entidad, IPermission
    {
        public string RecursoCodigo { get; set; }

        public virtual Recurso Recurso { get; set; }

        public string Accion { get; set; }

        public string Descripcion { get; set; }

        public virtual ICollection<Rol> Roles { get; set; }

        #region Auth

        public string Resource
        {
            get { return this.RecursoCodigo; }
        }

        public string Action
        {
            get { return this.Accion; }
        }

        #endregion

    }
}
