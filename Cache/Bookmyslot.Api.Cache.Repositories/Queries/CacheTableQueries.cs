namespace Bookmyslot.Api.Cache.Repositories.Queries
{

    public class CacheTableQueries
    {


     //   SELECT(CAST(CreatedDateUtc AS DATETIME) + CAST(ExpiryTime AS DATETIME)) as ExpiryTime
     //FROM Cache

        public const string GetCacheQuery = @"select CacheValue from Cache where ExpiryTimeUtc > SYSUTCDATETIME()
   and Id=@Id";


        public const string InsertCacheQuery = @"INSERT INTO Cache (Id, CacheValue, ExpiryTimeUtc, CreatedDateUtc)
  VALUES (@Id,@CacheValue,@ExpiryTimeUtc,@CreatedDateUtc)";

    }
}
