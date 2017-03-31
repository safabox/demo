using Gestion.Common.Domain.Seguridad;
using Gestion.Data.Extensions;

namespace Gestion.Data.Mappers.Seguridad
{
    class RolMapper : EntidadModificableMapper<Rol>
    {
        public RolMapper()
            : base("Roles")
        {
        }

        protected override void MapFields()
        {
            this.Property(c => c.Nombre)
                .IsRequired()
                .HasMaxLength(255)
                .HasUniqueIndexAnnotation("UK_Roles_Nombre");

            this.Property(c => c.Descripcion)
                .HasMaxLength(500);

            this.HasMany<Permiso>(x => x.Permisos)
                .WithMany(x => x.Roles)
                .Map(m => m
                    .ToTable("RolesPermisos")
                    .MapLeftKey("RolId")
                    .MapRightKey("PermisoId")
                );
        }
    }
}
