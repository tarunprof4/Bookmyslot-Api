using Bookmyslot.SharedKernel.ValueObject;
using System;
using System.Threading.Tasks;

namespace Bookmyslot.SharedKernel.Contracts.Cache
{

    public interface IDistributedDatabaseCacheBuisness
    {
        Task<Result<T>> GetFromCacheAsync<T>(CacheModel cacheModel, Func<Task<Result<T>>> retrieveValues, bool refresh = false) where T : class;
    }
}
