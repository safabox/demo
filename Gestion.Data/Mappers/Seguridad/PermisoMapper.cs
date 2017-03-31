using Gestion.Common.Domain.Seguridad;

namespace Gestion.Data.Mappers.Seguridad
{
    class PermisoMapper : EntidadMapper<Permiso>
    {
        public PermisoMapper()
            : base("Permisos")
        {
        }

        protected override void MapFields()
        {

            this.Property(p => p.Descripcion)
                .IsRequired()
                .HasMaxLength(500);

            this.HasRequired(p => p.Recurso)
                .WithMany(r => r.Permisos)
                .HasForeignKey(m => m.RecursoCodigo);
        }
    }
}
