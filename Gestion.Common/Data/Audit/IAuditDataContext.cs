using Gestion.Common.Domain.Audit;
using Gestion.Common.Domain.Seguridad;
using FrameLog.History;

namespace Gestion.Common.Data.Audit
{
    public interface IAuditDataContext
    {
        HistoryExplorer<ChangeSet, Usuario> HistoryExplorer { get; }
    }
}
