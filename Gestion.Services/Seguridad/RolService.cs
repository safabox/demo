using Gestion.Common.Utils;
using Gestion.Common.Data.Seguridad;
using Gestion.Common.Domain.Seguridad;
using Gestion.Common.Services.Seguridad;
using Gestion.Security;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion.Services.Seguridad
{
    public class RolService : BaseEntidadModificableService<Rol, IRolDataContext>, IRolService
    {
        private readonly IUserManager userManager;

        protected override string NotFoundMessage
        {
            get { return "Rol inexistente"; }
        }

        protected override DbSet<Rol> EntitySource
        {
            get { return base.context.Roles; }
        }

        public RolService(IRolDataContext context, ISecurityContext securityContext, IUserManager userManager)
            : base(context, securityContext)
        {
            this.userManager = userManager;
        }

        protected override Rol ExecuteAdd(Rol entity)
        {
            var entityDB = base.ExecuteAdd(entity);

            foreach (var permiso in entityDB.Permisos)
            {

                base.context.SetEntityState<Permiso>(permiso, EntityState.Unchanged);
            }

            return entityDB;
        }

        protected override bool ValidateAdd(Rol entity, out string[] validationErrors)
        {
            var errorList = new List<string>();

            if (!ValidarNombreUnico(entity.Nombre))
            {
                errorList.Add("El Nombre ya existe");
            }
            validationErrors = errorList.ToArray();
            return errorList.Count == 0;
        }

        protected override bool ValidateUpdate(long id, Rol entity, Rol entityDB, out string[] validationErrors)
        {
            var errorList = new List<string>();

            if (!ValidarNombreUnico(id, entity.Nombre))
            {
                errorList.Add("El Nombre ya existe");
            }
            validationErrors = errorList.ToArray();
            return errorList.Count == 0;
        }

        protected override void UpdateFields(Rol entityDB, Rol entity)
        {
            var permisosEliminados = entityDB.Permisos.Where(x => !entity.Permisos.Any(p => p.Id == x.Id)).ToList();
            var permisosNuevos = entity.Permisos.Where(x => !entityDB.Permisos.Any(p => p.Id == x.Id)).ToList();

            bool existenCambiosPermisos = permisosEliminados.Count > 0 || permisosNuevos.Count > 0;

            // Elimina los destildados
            foreach (var permiso in permisosEliminados)
            {
                entityDB.Permisos.Remove(permiso);
            }
            // Agrega nuevos tildados
            foreach (var permiso in permisosNuevos)
            {
                var permDB = context.Permisos.Find(permiso.Id);
                if (permDB != null)
                {
                    entityDB.Permisos.Add(permDB);
                }
            }
            entityDB.Nombre = entity.Nombre;
            entityDB.Descripcion = entity.Descripcion;

            base.UpdateFields(entityDB, entity);

            if (existenCambiosPermisos)
            {
                AsyncHelper.RunSync(() => EliminarSesiones(entityDB));
            }
        }

        public IQueryable<Membresia> GetMembresiasActivas(long idRol)
        {
            return this.context.Membresias
                .Where(x => x.Rol.Id == idRol)
                .Where(Membresia.ActivasPredicate());
        }

        private Task EliminarSesiones(Rol rol)
        {
            List<Task> tasks = new List<Task>();

            foreach (var membresia in rol.Membresias)
            {
                var task = this.userManager.InvalidateUserSession(membresia.Usuario.UserName);
                tasks.Add(task);
            }

            return Task.WhenAll(tasks);
        }

        private bool ValidarNombreUnico(string nombre)
        {
            return !(base.context.Roles.Any(x => x.Nombre.Equals(nombre, StringComparison.InvariantCultureIgnoreCase)));
        }

        private bool ValidarNombreUnico(long id, string nombre)
        {
            return !(base.context.Roles.Any(x => x.Nombre.Equals(nombre, StringComparison.InvariantCultureIgnoreCase) && x.Id != id));
        }

    }
}
