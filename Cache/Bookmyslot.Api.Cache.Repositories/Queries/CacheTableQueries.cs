namespace Bookmyslot.Api.Cache.Repositories.Queries
{

    public class CacheTableQueries
    {
   
        public const string GetCacheQuery = @"select CacheValue from Cache where CreatedDateUtc < SYSUTCDATETIME()";


        public const string InsertCacheQuery = @"INSERT INTO Cache (CacheKey, CacheType, CacheValue, ExpiryTime, CreatedDateUtc)
  VALUES (@CacheKey,@CacheType,@CacheValue,@ExpiryTime,@CreatedDateUtc)";

    }
}
