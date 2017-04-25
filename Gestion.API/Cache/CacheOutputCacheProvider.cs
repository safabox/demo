using Gestion.API.Controllers;
using Gestion.Common.Cache;
using System.Web.Http;
using WebApi.OutputCache.Core.Cache;
using WebApi.OutputCache.V2;

namespace Gestion.API.Cache
{
    public class CacheOutputCacheProvider : ICacheProvider
    {
        private readonly CacheOutputConfiguration cacheConfig;
        private readonly IApiOutputCache cache;

        public CacheOutputCacheProvider(IApiOutputCache cache)
        {
            this.cacheConfig = GlobalConfiguration.Configuration.CacheOutputConfiguration();
            this.cache = cache;
        }

        public void InvalidateUsuarios()
        {
            this.cache.RemoveStartsWith(cacheConfig.MakeBaseCachekey((UsuariosController c) => c.GetAll()));
        }
    }
}