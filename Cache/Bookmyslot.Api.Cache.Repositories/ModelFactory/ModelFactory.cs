using Bookmyslot.Api.Cache.Repositories.Enitites;

namespace Bookmyslot.Api.Cache.Repositories.ModelFactory
{
    internal class ModelFactory
    {
        internal static string CreateCacheValue(CacheEntity cacheEntity)
        {
            return cacheEntity.CacheValue;
        }
    }
}
