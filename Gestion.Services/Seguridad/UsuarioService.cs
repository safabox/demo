using Gestion.Common.Data.Seguridad;
using Gestion.Common.Domain.Seguridad;
using Gestion.Common.Services.Seguridad;
using Gestion.Common;
using Gestion.Security;
using Gestion.Security.PasswordPolicy;
using Gestion.Common.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion.Services.Seguridad
{
    public class UsuarioService : BaseEntidadModificableService<Usuario, IUsuarioDataContext>, IUsuarioService
    {
        private readonly IPasswordPolicy passwordPolicy;
        private readonly IUserManager userManager;

        protected override string NotFoundMessage
        {
            get { return "Usuario Inexistente"; }
        }

        protected override DbSet<Usuario> EntitySource
        {
            get { return this.context.Usuarios; }
        }

        public UsuarioService(IUsuarioDataContext context, IPasswordPolicy passwordPolicy, ISecurityContext securityContext, IUserManager userManager)
    :   base(context, securityContext)
        {
            this.passwordPolicy = passwordPolicy;
            this.userManager = userManager;
        }

        protected override bool ValidateAdd(Usuario entity, out string[] validationErrors)
        {
            var errorList = new List<string>();

            if (!ValidarNombreUsuarioUnico(entity.NombreUsuario))
            {
                errorList.Add("El Nombre de Usuario ya existe");
            }

            IEnumerable<string> passwordErrors;
            if (!ValidarPassword(entity.Password, out passwordErrors))
            {
                errorList.AddRange(passwordErrors);
            }

            validationErrors = errorList.ToArray();
            return errorList.Count == 0;
        }

        protected override Usuario ExecuteAdd(Usuario entity)
        {
            entity.NombreUsuario = entity.NombreUsuario.ToLower();
            NormalizarFechasMembresia(entity);

            var entityDB = base.ExecuteAdd(entity);

            bool forzarCambioPassword = entity.ForzarCambioPassword;
            entityDB.ChangePassword(entity.Password);
            entityDB.ForzarCambioPassword = forzarCambioPassword;

            return entityDB;
        }

        protected override bool ValidateUpdate(long id, Usuario entity, Usuario entityDB, out string[] validationErrors)
        {
            var errorList = new List<string>();

            if (!String.IsNullOrEmpty(entity.Password))
            {
                IEnumerable<string> passwordErrors;

                if (!ValidarPassword(entity.Password, out passwordErrors))
                {
                    errorList.AddRange(passwordErrors);
                }
            }
            validationErrors = errorList.ToArray();
            return errorList.Count == 0;
        }

        protected override void UpdateFields(Usuario entityDB, Usuario entity)
        {
            NormalizarFechasMembresia(entity);

            base.UpdateFields(entityDB, entity);

            bool changePassword = (!String.IsNullOrEmpty(entity.Password));

            // Elimina membresías borradas
            foreach (var membresiaDB in entityDB.Membresias.Where(db => !entity.Membresias.Any(e => e.RolId == db.RolId)).ToList())
            {
                entityDB.Membresias.Remove(membresiaDB);
            }

            // Agrega nuevas y modifica existentes
            foreach (var membresia in entity.Membresias)
            {
                var membresiaDB = entityDB.Membresias.FirstOrDefault(e => e.RolId == membresia.RolId);
                if (membresiaDB == null)
                {
                    var rolDB = context.Roles.Find(membresia.RolId);
                    membresiaDB = new Membresia
                    {
                        RolId = rolDB.Id,
                        Rol = rolDB,
                        UsuarioId = entityDB.Id,
                        Usuario = entityDB
                    };
                    entityDB.Membresias.Add(membresiaDB);
                }

                membresiaDB.VigenteDesde = membresia.VigenteDesde;
                membresiaDB.VigenteHasta = membresia.VigenteHasta;
            }

            entityDB.Nombre = entity.Nombre;
            entityDB.Apellido = entity.Apellido;
            entityDB.Correo = entity.Correo;
            entityDB.Telefono = entity.Telefono;

            if (changePassword)
            {
                entityDB.ChangePassword(entity.Password);
            }
            entityDB.ForzarCambioPassword = entity.ForzarCambioPassword;

            // Inhabilita la sesión del usuario
            AsyncHelper.RunSync(() => this.userManager.InvalidateUserSession(entityDB.UserName));
        }

        public override async Task<OperationResult<Usuario>> Delete(long id)
        {
            try
            {
                var result = await base.Delete(id);
                if (result.Succeeded)
                {
                    var entity = await GetAsync(id);
                    await this.userManager.InvalidateUserSession(entity.UserName);
                }
                return result;
            }
            catch (Exception ex)
            {
                return OperationResult<Usuario>.Failed(ex);
            }
        }

        private bool ValidarNombreUsuarioUnico(string nombreUsuario)
        {
            return !(this.context.Usuarios.Any(x => x.Nombre.Equals(nombreUsuario, StringComparison.InvariantCultureIgnoreCase)));
        }

        private bool ValidarPassword(string password, out IEnumerable<string> passwordErrors)
        {
            return this.passwordPolicy.IsValidPassword(password, out passwordErrors);
        }

        private void NormalizarFechasMembresia(Usuario entity)
        {
            foreach (var membresia in entity.Membresias)
            {
                membresia.VigenteDesde = membresia.VigenteDesde.Date;
                if (membresia.VigenteHasta.HasValue)
                {
                    membresia.VigenteHasta = membresia.VigenteHasta.Value.Date.AddDays(1).AddSeconds(-1);
                }
                else
                {
                    membresia.VigenteHasta = null;
                }
            }
        }
    }
}
