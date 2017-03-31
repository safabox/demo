using System;
using System.Collections.Generic;
using Gestion.Common.Domain.Seguridad;
using FrameLog.Models;


namespace Gestion.Common.Domain.Audit
{
    public class ObjectChange : IObjectChange<Usuario>
    {
        public long Id { get; set; }
        public string TypeName { get; set; }
        public string ObjectReference { get; set; }
        public virtual ChangeSet ChangeSet { get; set; }
        public virtual List<PropertyChange> PropertyChanges { get; set; }

        IEnumerable<IPropertyChange<Usuario>> IObjectChange<Usuario>.PropertyChanges
        {
            get { return PropertyChanges; }
        }

        void IObjectChange<Usuario>.Add(IPropertyChange<Usuario> propertyChange)
        {
            PropertyChanges.Add((PropertyChange)propertyChange);
        }

        IChangeSet<Usuario> IObjectChange<Usuario>.ChangeSet
        {
            get { return ChangeSet; }
            set { ChangeSet = (ChangeSet)value; }
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", TypeName, ObjectReference);
        }
    }
}
