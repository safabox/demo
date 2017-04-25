using Gestion.API.Models.Membresias;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gestion.API.Models.Usuarios
{
    public class PostUsuarioModel : Model
    {

        #region Datos Personales

        [Required(ErrorMessage = "{0} requerido")]
        [Display(Name = "Usuario")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "{0} requerido")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "{0} requerido")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "{0} requerido")]
        [EmailAddress(ErrorMessage = "Correo inválido")]
        public string Correo { get; set; }
        public string Telefono { get; set; }

        #endregion

        #region Seguridad

        [Required(ErrorMessage = "{0} requerida")]
        [DataType(DataType.Password)]
        [Display(Name = "Clave")]
        public string Password { get; set; }

        [Required(ErrorMessage = "{0} requerida")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmación de nueva clave")]
        [Compare("Password", ErrorMessage = "La clave y la confirmación no coinciden")]
        public string ConfirmPassword { get; set; }

        public bool ForzarCambioPassword { get; set; }

        #endregion

        #region Membresías
        public IEnumerable<PostMembresiaModel> Membresias { get; set; }
        #endregion
    }
}