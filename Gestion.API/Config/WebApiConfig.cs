using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace Gestion.API.Config
{
    internal static class WebApiConfig
    {
        internal static void Configure(IAppBuilder app, HttpConfiguration config)
        {
            // Web API routes
            RegisterRoutes(config);

            // Quitar response de tipo Xml
            RemoveXmlFormatters(config);

            // Json Formatter
            EnableCamelCaseJSONResponse(config);

            // Date Format
            SetJSONDateFormat(config);

            app.UseWebApi(config);
        }

        private static void RemoveXmlFormatters(HttpConfiguration config)
        {
            config.Formatters.Remove(config.Formatters.XmlFormatter);
        }

        private static void RegisterRoutes(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        private static void EnableCamelCaseJSONResponse(HttpConfiguration config)
        {
            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        private static void SetJSONDateFormat(HttpConfiguration config)
        {
            config.Formatters.JsonFormatter.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
        }
    }
}
