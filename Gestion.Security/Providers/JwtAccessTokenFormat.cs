using Gestion.Security.Factories;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using System;
using System.IdentityModel.Tokens;
using Thinktecture.IdentityModel.Tokens;

namespace Gestion.Security.Providers
{
    class JwtAccessTokenFormat : ISecureDataFormat<AuthenticationTicket>
    {
        private const string AudiencePropertyKey = "audience";

        private IAudiencesStoreFactory audiencesStoreFactory;

        public string Issuer { get; set; }

        public JwtAccessTokenFormat(IAudiencesStoreFactory audiencesStoreFactory)
        {
            this.audiencesStoreFactory = audiencesStoreFactory;
        }

        public string Protect(AuthenticationTicket data)
        {
            using (var audiencesStore = this.audiencesStoreFactory.Create())
            {
                if (data == null)
                {
                    throw new ArgumentNullException("data");
                }

                string audienceId = data.Properties.Dictionary.ContainsKey(AudiencePropertyKey) ? data.Properties.Dictionary[AudiencePropertyKey] : null;

                if (string.IsNullOrWhiteSpace(audienceId)) throw new InvalidOperationException("AuthenticationTicket.Properties does not include audience");

                var audience = audiencesStore.Find(audienceId);

                string symmetricKeyAsBase64 = audience.Base64Secret;

                var keyByteArray = TextEncodings.Base64Url.Decode(symmetricKeyAsBase64);

                var signingKey = new HmacSigningCredentials(keyByteArray);

                var issued = data.Properties.IssuedUtc;
                var expires = data.Properties.ExpiresUtc;

                var token = new JwtSecurityToken(this.Issuer, audienceId, data.Identity.Claims, issued.Value.UtcDateTime, expires.Value.UtcDateTime, signingKey);

                var handler = new JwtSecurityTokenHandler();

                var jwt = handler.WriteToken(token);

                return jwt;
            }
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            throw new NotImplementedException();
        }
    }
}

