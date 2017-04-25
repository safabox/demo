using System;
using System.Threading.Tasks;
using Gestion.Common.Domain.Auth;
using Gestion.Common;

namespace Gestion.Security.Stores
{
    public interface IRefreshTokenStore : IDisposable
    {
        Task<OperationResult> Add(RefreshToken token);

        Task<RefreshToken> FindAsync(string tokenId);

        Task<OperationResult> Remove(string tokenId);
    }
}
