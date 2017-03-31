using System;
using System.Collections.Generic;
using Gestion.Common.Utils;

namespace Gestion.Common.Domain.Seguridad
{
    public class Usuario : EntidadModificable
    {
        #region Datos Personales

        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }

        #endregion

        #region Seguridad

        public string NombreUsuario { get; set; }

        public string Password { get; set; }
        public bool ForzarCambioPassword { get; set; }

        #endregion

        #region Membresías

        public virtual ICollection<Membresia> Membresias { get; set; }

        #endregion

        #region Modificacion/Estado

        public bool Activo { get; set; }

        #endregion

        public Usuario()
        {
            this.Membresias = new List<Membresia>();
            this.Activo = true;
            this.FechaCreacion = DateTime.Now;
        }

        #region Auth

        public string UserName
        {
            get
            {
                return this.NombreUsuario;
            }
        }

        public string Sid
        {
            get
            {
                return this.Id.ToString();
            }
        }

        public bool PasswordExpired
        {
            get
            {
                return this.ForzarCambioPassword;
            }
        }

        public string DisplayName
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Apellido) && !string.IsNullOrEmpty(this.Nombre))
                {
                    return string.Concat(this.Apellido, ", ", this.Nombre);
                }
                return this.NombreUsuario;
            }
        }

        public bool ValidatePassword(string password)
        {
            return CryptographyUtils.ValidatePassword(password, this.Password);
        }

        public void ChangePassword(string newPassword)
        {
            this.Password = CryptographyUtils.CreatePasswordHash(newPassword);
            this.ForzarCambioPassword = false;
        }

        #endregion
    }
}
