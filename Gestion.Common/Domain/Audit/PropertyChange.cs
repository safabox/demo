using Gestion.Common.Domain.Seguridad;
using FrameLog.Models;

namespace Gestion.Common.Domain.Audit
{
    public class PropertyChange : IPropertyChange<Usuario>
    {
        public long Id { get; set; }
        public virtual ObjectChange ObjectChange { get; set; }
        public string PropertyName { get; set; }
        public string Value { get; set; }
        public int? ValueAsInt { get; set; }

        IObjectChange<Usuario> IPropertyChange<Usuario>.ObjectChange
        {
            get { return ObjectChange; }
            set { ObjectChange = (ObjectChange)value; }
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", PropertyName, Value);
        }
    }
}
