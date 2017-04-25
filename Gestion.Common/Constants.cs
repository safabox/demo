
namespace Gestion
{
    public class Constants
    {

        public class Cache
        {
            public const int ENTITY_DEFAULT_SERVERTIMESPAN = 900;
        }

        public class AppSettings
        {
            // Authorization Server
            public const string AUTH_SERVER_TOKEN_ISSUER = "AuthServerTokenIssuer";
            public const string AUTH_SERVER_ALLOW_INSECURE_HTTP = "AuthServerAllowInsecureHTTP";
            public const string AUTH_SERVER_ACCESS_TOKEN_EXPIRATION_SECONDS = "AuthServerAccessTokenExpirationsSeconds";
            // SMTP
            public const string SMTP_HOST = "SMTPHost";
            public const string SMTP_PORT = "SMTPPort";
            public const string SMTP_UseDefaultCredentials = "SMTPUseDefaultCredentials";
            public const string SMTP_FromAddress = "SMTPFromAddress";
            public const string SMTP_UserName = "SMTPUserName";
            public const string SMTP_Password = "SMTPPassword";
            public const string SMTP_Domain = "SMTPDomain";
            // Jobs
            public const string JOB_SCHEDULE_PREFIX = "Schedule";
            // Generador PDF
            public const string GENERADOR_PDF_OFERTA_HYPERVINCULO_BASE = "GeneradorPDFOfertaHyperVinculoBase";
        }
    }
}
