using Gestion.API.Config;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http;

[assembly: OwinStartup(typeof(Gestion.API.Startup))]
namespace Gestion.API
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //Se utiliza para configurar la ruta del API
            var config = new HttpConfiguration();

            // Habilitar CORS - Para evitar Error No ‘Access-Control-Allow-Origin’
            app.UseCors(GetCorsOptions());

            // DI & IoC
            DIConfig.Configure(app, config);

            // Auth
            AuthConfig.ConfigureAuthServer(app, config);
            AuthConfig.ConfigureResourceServer(app);

            // WebApi
            WebApiConfig.Configure(app, config);

            // Cache
            OutputCacheConfig.Configure(app, config);

        }

        private CorsOptions GetCorsOptions()
        {
            var policy = new CorsPolicy
            {
                AllowAnyHeader = true,
                AllowAnyMethod = true,
                AllowAnyOrigin = true,
                SupportsCredentials = true,
            };

            var exposedHeaders = new string[]
            {
                "ETag",
                "x-filename",
                "Content-Disposition",
                "Content-Type",
                "Accept",
                "Content-Type",
                "If-Match",
                "If-None-Match",
                "If-Unmodified-Since",
                "If-Modified-Since"
            };

            foreach (var header in exposedHeaders)
            {
                policy.ExposedHeaders.Add(header);
            }

            return new CorsOptions()
            {
                PolicyProvider = new CorsPolicyProvider
                {
                    PolicyResolver = context => Task.FromResult(policy),
                },
            };
        }
    }
}