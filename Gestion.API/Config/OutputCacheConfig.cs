using Owin;
using System.Web.Http;
using WebApi.OutputCache.Core.Cache;
using WebApi.OutputCache.V2;

namespace Gestion.API.Config
{
    internal static class OutputCacheConfig
    {
        internal static void Configure(IAppBuilder app, HttpConfiguration config)
        {
            config.CacheOutputConfiguration().RegisterCacheOutputProvider(() => new MemoryCacheDefault());
        }
    }
}