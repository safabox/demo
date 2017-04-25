using Gestion.Security.Providers;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Configuration;

namespace Gestion.Security.Factories
{
    public class GestionOAuthAuthorizationServerOptionsFactory : IOAuthAuthorizationServerOptionsFactory
    {
        private const string TOKEN_ENDPOINT = "/oauth2/token";

        private IOAuthAuthorizationServerProvider _authorizationProvider;
        private IAudiencesStoreFactory _audiencesStoreFactory;
        private IRefreshTokenStoreFactory _refreshTokenStoreFactory;

        public bool AllowInsecureHttp { get; set; }
        public string TokenEndpointPath { get; set; }
        public TimeSpan AccessTokenExpireTimeSpan { get; set; }
        public string JwtAccessTokenIssuer { get; set; }

        public GestionOAuthAuthorizationServerOptionsFactory(IOAuthAuthorizationServerProvider authorizationProvider, IAudiencesStoreFactory audiencesStoreFactory, IRefreshTokenStoreFactory refreshTokenStoreFactory)
        {
            _authorizationProvider = authorizationProvider;
            _audiencesStoreFactory = audiencesStoreFactory;
            _refreshTokenStoreFactory = refreshTokenStoreFactory;

            var allowInsecureHttp = Convert.ToBoolean(ConfigurationManager.AppSettings[Constants.AppSettings.AUTH_SERVER_ALLOW_INSECURE_HTTP]);
            var accessTokenExpirationSeconds = Convert.ToDouble(ConfigurationManager.AppSettings[Constants.AppSettings.AUTH_SERVER_ACCESS_TOKEN_EXPIRATION_SECONDS]);
            var tokenIssuer = ConfigurationManager.AppSettings[Constants.AppSettings.AUTH_SERVER_TOKEN_ISSUER];

            this.AllowInsecureHttp = allowInsecureHttp;
            this.AccessTokenExpireTimeSpan = TimeSpan.FromSeconds(accessTokenExpirationSeconds);
            this.JwtAccessTokenIssuer = tokenIssuer;
            this.TokenEndpointPath = TOKEN_ENDPOINT;
        }

        public OAuthAuthorizationServerOptions GetOptions()
        {
            return new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = this.AllowInsecureHttp,
                TokenEndpointPath = new PathString(this.TokenEndpointPath),
                AccessTokenExpireTimeSpan = this.AccessTokenExpireTimeSpan,
                Provider = _authorizationProvider,
                AccessTokenFormat = new JwtAccessTokenFormat(_audiencesStoreFactory)
                {
                    Issuer = this.JwtAccessTokenIssuer
                },
                RefreshTokenProvider = new GestionRefreshTokenProvider(this._refreshTokenStoreFactory)
            };
        }
    }
}
