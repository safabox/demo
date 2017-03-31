using Gestion.Common.Domain;

namespace Gestion.Data.Mappers
{
    abstract class EntidadModificableMapper<T> : EntidadMapper<T> where T : EntidadModificable
    {
        public EntidadModificableMapper(string tableName)
            : base(tableName)
        {
            this.MapModificacionFields();
        }

        private void MapModificacionFields()
        {
            this.Property(x => x.FechaCreacion)
                .IsRequired();

            this.HasRequired(x => x.UsuarioCreacion)
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
