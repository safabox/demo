using Gestion.Common.Domain.Seguridad;
using System.Data.Entity.ModelConfiguration;

namespace Gestion.Data.Mappers.Seguridad
{
    class RecursoMapper : EntityTypeConfiguration<Recurso>
    {
        public RecursoMapper()
        {
            this.ToTable("Recursos");

            this.HasKey(r => r.Codigo);

            this.Property(r => r.Codigo)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(r => r.Descripcion)
                .IsRequired()
                .HasMaxLength(200);
        }
    }
}
