namespace Bookmyslot.Api.Cache.Repositories.Queries
{

    public class CacheTableQueries
    {
   
        public const string GetCacheQuery = @"select CacheValue from Cache where CreatedDateUtc < SYSUTCDATETIME()";

    }
}
