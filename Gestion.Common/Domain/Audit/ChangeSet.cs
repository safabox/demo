using Gestion.Common.Domain.Seguridad;
using FrameLog.Models;
using System;
using System.Collections.Generic;

namespace Gestion.Common.Domain.Audit
{
    public class ChangeSet : IChangeSet<Usuario>
    {
        public long Id { get; set; }
        public string Comments { get; set; }
        public DateTime Timestamp { get; set; }
        public Usuario Author { get; set; }
        public virtual List<ObjectChange> ObjectChanges { get; set; }

        IEnumerable<IObjectChange<Usuario>> IChangeSet<Usuario>.ObjectChanges
        {
            get { return ObjectChanges; }
        }

        void IChangeSet<Usuario>.Add(IObjectChange<Usuario> objectChange)
        {
            ObjectChanges.Add((ObjectChange)objectChange);
        }

        public override string ToString()
        {
            return string.Format("By {0} on {1}, with {2} ObjectChanges",
                Author, Timestamp, ObjectChanges.Count);
        }
    }
}
