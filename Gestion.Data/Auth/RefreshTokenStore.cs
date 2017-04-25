using Gestion.Common;
using Gestion.Common.Domain.Auth;
using Gestion.Common.Data.Auth;
using Gestion.Security.Stores;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion.Data.Auth
{
    public class RefreshTokenStore : IRefreshTokenStore
    {
        private readonly ISecurityDataContext context;

        public RefreshTokenStore(ISecurityDataContext context)
        {
            this.context = context;
        }

        public async Task<OperationResult> Add(RefreshToken token)
        {
            try
            {
                // Elimina refresh token existentes del mismo usuario y audience
                var existingTokens = this.context.RefreshTokens.Where(r => r.Subject == token.Subject && r.AudienceId == token.AudienceId);
                this.context.RefreshTokens.RemoveRange(existingTokens);

                this.context.RefreshTokens.Add(token);
                await this.context.SaveChangesAsync();
                return OperationResult.Success();
            }
            catch (Exception ex)
            {
                return OperationResult.Failed(ex);
            }
        }

        public Task<RefreshToken> FindAsync(string tokenId)
        {
            return this.context.RefreshTokens.FindAsync(tokenId);
        }

        public async Task<OperationResult> Remove(string tokenId)
        {
            try
            {
                var token = await this.context.RefreshTokens.FindAsync(tokenId);
                if (token != null)
                {
                    this.context.RefreshTokens.Remove(token);
                    await this.context.SaveChangesAsync();
                    return OperationResult.Success();
                }
                return OperationResult.Failed("Token inexistente");
            }
            catch (Exception ex)
            {
                return OperationResult.Failed(ex);
            }
        }

        public void Dispose()
        {
        }
    }
}
