namespace Bookmyslot.Api.Cache.Repositories.Queries
{

    public class CacheTableQueries
    {


     //   SELECT(CAST(CreatedDateUtc AS DATETIME) + CAST(ExpiryTime AS DATETIME)) as ExpiryTime
     //FROM Cache

        public const string GetCacheQuery = @"select CacheValue from Cache where ExpiryTimeUtc > SYSUTCDATETIME()";


        public const string InsertCacheQuery = @"INSERT INTO Cache (CacheKey, CacheType, CacheValue, ExpiryTimeUtc, CreatedDateUtc)
  VALUES (@CacheKey,@CacheType,@CacheValue,@ExpiryTimeUtc,@CreatedDateUtc)";

    }
}
