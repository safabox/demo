using Gestion.API.Cache;
using Gestion.Common.Cache;
using Autofac;
using WebApi.OutputCache.Core.Cache;

namespace Gestion.API.Config.DIModules
{
    public class CacheDIModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CacheOutputCacheProvider>().As<ICacheProvider>();

            builder.RegisterType<MemoryCacheDefault>().As<IApiOutputCache>()
                .SingleInstance();
        }
    }
}