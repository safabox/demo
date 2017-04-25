using Gestion.Common.Domain.Auth;
using Gestion.Common.Data.Auth;
using Gestion.Security.Stores;
using System.Threading.Tasks;

namespace Gestion.Data.Auth
{
    public class AudiencesStore : IAudiencesStore
    {
        private readonly ISecurityDataContext context;

        public AudiencesStore(ISecurityDataContext context)
        {
            this.context = context;
        }

        public Task<Audience> FindAsync(string audienceId)
        {
            return this.context.Audiences.FindAsync(audienceId);
        }

        public Audience Find(string audienceId)
        {
            return this.context.Audiences.Find(audienceId);
        }

        public void Dispose()
        {
        }
    }
}
