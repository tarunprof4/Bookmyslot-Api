using Bookmyslot.Api.Cache.Contracts;
using Bookmyslot.Api.Cache.Repositories.Enitites;
using System;

namespace Bookmyslot.Api.Cache.Repositories.EntityFactory
{
    internal class EntityFactory
    {
        internal static CacheEntity CreateCacheKeyEntity(CacheModel cacheModel)
        {
            return new CacheEntity()
            {
                CacheKey = cacheModel.Key,
                CacheType = cacheModel.Type,
                CacheValue = cacheModel.Value,
                ExpiryTimeUtc = DateTime.UtcNow.Add(cacheModel.ExpiryTimeUtc),
                CreatedDateUtc = DateTime.UtcNow
            };
        }


    }
}
