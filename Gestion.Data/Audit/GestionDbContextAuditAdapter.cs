using Gestion.Common.Domain.Seguridad;
using Gestion.Common.Domain.Audit;
using FrameLog.Contexts;
using FrameLog.Models;
using System;
using System.Linq;

namespace Gestion.Data.Audit
{
    class GestionDbContextAuditAdapter : DbContextAdapter<ChangeSet, Usuario>
    { 

        private GestionDbContext context;

        public GestionDbContextAuditAdapter(GestionDbContext context)
                : base(context)
        {
            this.context = context;
        }

        public override IQueryable<IChangeSet<Usuario>> ChangeSets
        {
            get
            {
                return this.context.ChangeSets.Include("Author");
            }
        }

        public override IQueryable<IObjectChange<Usuario>> ObjectChanges
        {
            get
            {
                return this.context.ObjectChanges.Include("ChangeSet.Author");
            }
        }

        public override IQueryable<IPropertyChange<Usuario>> PropertyChanges
        {
            get
            {
                return this.context.PropertyChanges;
            }
        }

        public override Type UnderlyingContextType
        {
            get
            {
                return typeof(GestionDbContext);
            }
        }

        public override void AddChangeSet(ChangeSet changeSet)
        {
            this.context.ChangeSets.Add(changeSet);
        }
    }
}
