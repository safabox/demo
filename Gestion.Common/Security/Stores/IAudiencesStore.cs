using System;
using System.Threading.Tasks;
using Gestion.Common.Domain.Auth;

namespace Gestion.Security.Stores
{
    public interface IAudiencesStore : IDisposable
    {
        Task<Audience> FindAsync(string audienceId);
        Audience Find(string audienceId);
    }
}
