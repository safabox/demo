using Gestion.Common.Domain.Audit;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Gestion.Data.Mappers.Audit
{
    class ObjectChangeMapper : EntityTypeConfiguration<ObjectChange>
    {
        public ObjectChangeMapper()
        {
            this.ToTable("ObjectChanges", "Audit");

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
            this.HasMany(x => x.PropertyChanges)
                .WithRequired(x => x.ObjectChange);
        }
    }
}
