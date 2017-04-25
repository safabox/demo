using Microsoft.Owin;

namespace Gestion.Security
{
    internal static class Helper
    {
        internal static void AddCORSHeades(IOwinContext owinContext, string allowedOrigin = "*")
        {
            if (owinContext.Response.Headers.ContainsKey("Access-Control-Allow-Origin"))
            {
                owinContext.Response.Headers.Remove("Access-Control-Allow-Origin");
            }
            owinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });
        }
    }
}
