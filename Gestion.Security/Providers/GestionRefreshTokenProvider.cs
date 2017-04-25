using Gestion.Common.Domain.Auth;
using Gestion.Security.Factories;
using Gestion.Common.Utils;
using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Threading.Tasks;

namespace Gestion.Security.Providers
{
    public class GestionRefreshTokenProvider : IAuthenticationTokenProvider
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly IRefreshTokenStoreFactory refreshTokenStoreFactory;

        public GestionRefreshTokenProvider(IRefreshTokenStoreFactory refreshTokenStoreFactory)
        {
            this.refreshTokenStoreFactory = refreshTokenStoreFactory;
        }

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            try
            {
                logger.Debug("CreateAsync - Inicio");

                using (var refreshTokenStore = this.refreshTokenStoreFactory.Create())
                {
                    var audienceId = context.Ticket.Properties.Dictionary["audience"];

                    if (string.IsNullOrEmpty(audienceId))
                    {
                        return;
                    }

                    var refreshTokenId = Guid.NewGuid().ToString("n");

                    var refreshTokenLifeTime = context.OwinContext.Get<string>("as:clientRefreshTokenLifeTime");

                    var token = new RefreshToken()
                    {
                        Id = CryptographyUtils.GetSHA256Hash(refreshTokenId),
                        AudienceId = audienceId,
                        Subject = context.Ticket.Identity.Name,
                        IssuedUtc = DateTime.UtcNow,
                        ExpiresUtc = DateTime.UtcNow.AddSeconds(Convert.ToDouble(refreshTokenLifeTime))
                    };

                    context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
                    context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;

                    token.ProtectedTicket = context.SerializeTicket();

                    var result = await refreshTokenStore.Add(token);

                    if (result.Succeeded)
                    {
                        logger.Debug("CreateAsync - Asigna Token");
                        context.SetToken(refreshTokenId);
                    }
                    else
                    {
                        throw new Exception(string.Join(" - ", result.Errors));
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "CreateAsync");
            }
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            try
            {
                logger.Debug("ReceiveAsync - Inicio");
                using (var refreshTokenStore = this.refreshTokenStoreFactory.Create())
                {
                    var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");

                    Helper.AddCORSHeades(context.OwinContext, allowedOrigin);

                    string hashedTokenId = CryptographyUtils.GetSHA256Hash(context.Token);

                    var refreshToken = await refreshTokenStore.FindAsync(hashedTokenId);

                    if (refreshToken != null)
                    {
                        logger.Debug("ReceiveAsync - Existe Token");
                        //Get protectedTicket from refreshToken class
                        context.DeserializeTicket(refreshToken.ProtectedTicket);
                        var result = await refreshTokenStore.Remove(hashedTokenId);
                        if (!result.Succeeded)
                        {
                            throw new Exception(string.Join(" - ", result.Errors));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "ReceiveAsync");
            }
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }

        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }
    }
}
