using Gestion.Common.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Gestion.Data.Mappers
{
    abstract class EntidadMapper<T> : EntityTypeConfiguration<T> where T : Entidad
    {
        protected abstract void MapFields();

        public EntidadMapper(string tableName)
        {
            this.ToTable(tableName);

            this.MapId();
            this.MapFields();
        }

        protected virtual void MapId()
        {
            this.HasKey(c => c.Id);

            this.Property(c => c.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .IsRequired();
        }
    }
}
