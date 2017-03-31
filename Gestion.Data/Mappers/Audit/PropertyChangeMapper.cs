using Gestion.Common.Domain.Audit;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Gestion.Data.Mappers.Audit
{
    class PropertyChangeMapper : EntityTypeConfiguration<PropertyChange>
    {
        public PropertyChangeMapper()
        {
            this.ToTable("PropertyChanges", "Audit");

            this.MapId();
            this.MapFields();
        }

        private void MapId()
        {
            this.HasKey(c => c.Id);

            this.Property(c => c.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .IsRequired();
        }

        private void MapFields()
        {
            this.Property(x => x.PropertyName)
                .HasMaxLength(256)
                .IsRequired();
        }
    }
}
