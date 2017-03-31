using Gestion.Common.Domain.Seguridad;
using Gestion.Common.Domain.Audit;
using System.Collections.Generic;
using FrameLog.Models;

namespace Gestion.Data.Audit
{
    public class ChangeSetFactory : IChangeSetFactory<ChangeSet, Usuario>
    {
        public ChangeSet ChangeSet()
        {
            var set = new ChangeSet();
            set.ObjectChanges = new List<ObjectChange>();
            return set;
        }

        public IObjectChange<Usuario> ObjectChange()
        {
            var o = new ObjectChange();
            o.PropertyChanges = new List<PropertyChange>();
            return o;
        }

        public IPropertyChange<Usuario> PropertyChange()
        {
            return new PropertyChange();
        }
    }
}
