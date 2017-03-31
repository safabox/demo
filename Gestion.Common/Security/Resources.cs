using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestion.Security
{
    public class Resources
    {
        #region Roles

        public const string Roles = "roles";

        public class RolesActions
        {
            public const string Listar = "l";
            public const string Editar = "e";
            public const string Crear = "c";
            public const string Eliminar = "d";
        }

        #endregion

        #region Usuarios

        public const string Usuarios = "usuarios";
        public class UsuariosActions
        {
            public const string Listar = "l";
            public const string Editar = "e";
            public const string EditarInfoPersonal = "eip";
            public const string EditarRoles = "er";
            public const string Crear = "c";
            public const string Eliminar = "d";
            public const string BatchUpdate = "b";
        }

        #endregion
    }
}
