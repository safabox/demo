using Gestion.Common.Domain.Seguridad;
using Gestion.Data.Extensions;

namespace Gestion.Data.Mappers.Seguridad
{
    class UsuarioMapper : EntidadModificableMapper<Usuario>
    {
        public UsuarioMapper()
            : base("Usuarios")
        {
        }

        protected override void MapFields()
        {
            this.Property(c => c.NombreUsuario)
                .IsRequired()
                .HasMaxLength(100)
                .HasUniqueIndexAnnotation("UK_Usuarios_NombreUsuario");

            this.Property(c => c.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(c => c.Apellido)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(c => c.Correo)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(x => x.FechaCreacion)
                .IsRequired();

            this.HasOptional(x => x.UsuarioCreacion)
                .WithMany()
                .HasForeignKey(fk => fk.UsuarioIdCreacion)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.UsuarioUltimaModificacion)
                .WithMany()
                .HasForeignKey(fk => fk.UsuarioIdUltimaModificacion)
                .WillCascadeOnDelete(false);

            this.Property(x => x.FechaUltimaModificacion)
                .IsOptional();

        }
    }
}
