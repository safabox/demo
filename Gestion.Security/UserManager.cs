using Gestion.Common;
using Gestion.Common.Data.Auth;
using Gestion.Common.Domain.Auth;
using Gestion.Common.Domain.Seguridad;
using Gestion.Security.PasswordPolicy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion.Security
{
    public class UserManager : IUserManager
    {
        private readonly ISecurityDataContext context;
        private readonly IPasswordPolicy passwordPolicy;

        public UserManager(ISecurityDataContext context, IPasswordPolicy passwordPolicy)
        {
            this.context = context;
            this.passwordPolicy = passwordPolicy;
        }

        public IUser GetByUserName(string userName)
        {
            var user = this.context.Usuarios.FirstOrDefault(x => x.NombreUsuario == userName);
            if (user != null)
            {
                this.context.Reload(user);
            }
            return user;
        }

        public async Task<OperationResult> ChangeUserPasswordAsync(string userName, string oldPassword, string newPassword)
        {
            try
            {
                IEnumerable<string> errors;

                if (this.passwordPolicy.IsValidPassword(newPassword, out errors))
                {
                    var usuario = GetByUserName(userName);
                    if (usuario != null)
                    {
                        if (usuario.ValidatePassword(oldPassword))
                        {
                            usuario.ChangePassword(newPassword);

                            // Elimina tokens del usuario
                            this.RemoveUserRefreshTokens(usuario.UserName);

                            await this.context.SaveChangesAsync();
                            return OperationResult.Success();
                        }
                        else
                        {
                            return OperationResult.Failed("La clave actual es incorrecta");
                        }
                    }
                    else
                    {
                        return OperationResult.Failed("Usuario inexistente");
                    }
                }
                else
                {
                    return OperationResult.Failed(errors.ToArray());
                }
            }
            catch (Exception ex)
            {
                return OperationResult.Failed(ex.Message);
            }
        }

        public IEnumerable<IPermission> GetPermissionsForUser(string userName)
        {
            var now = DateTime.UtcNow;

            var permisos = this.context.Membresias
                .Where(x => x.Usuario.NombreUsuario == userName)
                .Where(Membresia.ActivasPredicate())
                .SelectMany(x => x.Rol.Permisos)
                .AsQueryable<IPermission>()
                .Distinct()
                .ToList();

            return permisos;
        }

        public Task<OperationResult> InvalidateUserSession(string userName)
        {
            try
            {
                // Elimina tokens del usuario
                this.RemoveUserRefreshTokens(userName);
                return Task.FromResult(OperationResult.Success());
            }
            catch (Exception ex)
            {
                return Task.FromResult(OperationResult.Failed(ex.Message));
            }
        }

        private void RemoveUserRefreshTokens(string userName)
        {
            this.context.RefreshTokens.RemoveRange(this.context.RefreshTokens.Where(x => x.Subject == userName));
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~UserManager()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
            }
        }

        #endregion
    }
}
