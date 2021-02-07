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
                ExpiryTime = new TimeSpan(0,0, cacheModel.ExpiryInSeconds),
                CreatedDateUtc = DateTime.UtcNow
            };
        }


    }
}
