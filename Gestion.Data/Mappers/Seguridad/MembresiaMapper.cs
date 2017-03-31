using System.Data.Entity.ModelConfiguration;
using Gestion.Common.Domain.Seguridad;

namespace Gestion.Data.Mappers.Seguridad
{
    class MembresiaMapper : EntityTypeConfiguration<Membresia>
    {
        public MembresiaMapper()
        {
            this.ToTable("Membresias");

            this.MapId();
            this.MapFields();
        }

        protected void MapId()
        {
            this.HasKey(pk => new { pk.RolId, pk.UsuarioId });
        }

        protected void MapFields()
        {

            this.Property(c => c.VigenteDesde)
                .IsRequired();

            this.Property(c => c.VigenteHasta)
                .IsOptional();

            this.HasRequired(m => m.Rol)
                .WithMany(r => r.Membresias)
                .HasForeignKey(m => m.RolId);

            this.HasRequired(m => m.Usuario)
                .WithMany(u => u.Membresias)
                .HasForeignKey(m => m.UsuarioId);
        }
    }
}
