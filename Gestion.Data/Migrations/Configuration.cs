﻿namespace Gestion.Data.Migrations
{
    using Gestion.Common.Domain.Seguridad;
    using System.Data.Entity.Migrations;
    using Gestion.Security;
    using System.Linq;
    using Gestion.Common.Utils;


    internal sealed class Configuration : DbMigrationsConfiguration<GestionDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(GestionDbContext context)
        {
            SeedSeguridad(context);
        }

        #region Seguridad / Auth

        private void SeedSeguridad(GestionDbContext context)
        {
            #region Recursos y Permisos

            var recursos = new Recurso[] {
                new Recurso { Codigo = Resources.Roles, Descripcion = "ABM Roles" },
                new Recurso { Codigo = Resources.Usuarios, Descripcion = "ABM Usuarios" },
            };

            // Permisos de Seguridad
            var permisosSeguridad = new Permiso[] {
                // Roles
                new Permiso { RecursoCodigo = Resources.Roles, Accion = Resources.RolesActions.Listar, Descripcion = "Listar Roles" },
                new Permiso { RecursoCodigo = Resources.Roles, Accion = Resources.RolesActions.Editar, Descripcion = "Editar Roles" },
                new Permiso { RecursoCodigo = Resources.Roles, Accion = Resources.RolesActions.Crear, Descripcion = "Crear Roles" },
                new Permiso { RecursoCodigo = Resources.Roles, Accion = Resources.RolesActions.Eliminar, Descripcion = "Eliminar Roles" },
                // Usuarios
                new Permiso { RecursoCodigo = Resources.Usuarios, Accion = Resources.UsuariosActions.Listar, Descripcion = "Listar Usuarios" },
                new Permiso { RecursoCodigo = Resources.Usuarios, Accion = Resources.UsuariosActions.EditarInfoPersonal, Descripcion = "Editar Información Personal de Usuarios" },
                new Permiso { RecursoCodigo = Resources.Usuarios, Accion = Resources.UsuariosActions.EditarRoles, Descripcion = "Editar Roles de Usuarios" },
                new Permiso { RecursoCodigo = Resources.Usuarios, Accion = Resources.UsuariosActions.Crear, Descripcion = "Crear Usuarios" },
                new Permiso { RecursoCodigo = Resources.Usuarios, Accion = Resources.UsuariosActions.Eliminar, Descripcion = "Eliminar Usuarios" },
                new Permiso { RecursoCodigo = Resources.Usuarios, Accion = Resources.UsuariosActions.BatchUpdate, Descripcion = "Actualización masiva de Usuarios/Domicilios" },
            };

            #endregion

            // ************** Agrega Recursos **************
            context.Recursos.AddOrUpdate(r => r.Codigo, recursos);

            // ************** Agrega Permisos **************
            context.Permisos.AddOrUpdate(p => new { p.RecursoCodigo, p.Accion }, permisosSeguridad);
            context.SaveChanges();

            // ************** Rol Administrador Seguridad **************
            var admin = context.Usuarios.First(x => x.NombreUsuario == "admin");

            context.Usuarios.AddOrUpdate(x => x.NombreUsuario, admin);
            context.SaveChanges();

            if (context.Roles.Count() == 0)
            {
                var rolAdmin = new Rol
                {
                    Nombre = "Administrador Seguridad",
                    Descripcion = "Rol para administración de Usuarios y Roles",
                    UsuarioCreacion = admin,
                    Permisos = permisosSeguridad
                };
                context.Roles.AddOrUpdate(r => r.Nombre, rolAdmin);

                // ************** Actualiza Usuario Admin **************
                admin.UsuarioIdCreacion = admin.Id;

                if (admin.Membresias.Count == 0)
                {
                    admin.Membresias.Add(new Membresia
                    {
                        Rol = rolAdmin,
                        Usuario = admin
                    });
                }
                context.Usuarios.AddOrUpdate(x => x.NombreUsuario, admin);
                context.SaveChanges();
            }

        }
        
        #endregion



    }

}
