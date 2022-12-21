using Bookmyslot.SharedKernel.ValueObject;
using System;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Cache.Contracts.Interfaces
{

    public interface IDistributedInMemoryCacheBuisness
    {
        Task<Result<T>> GetFromCacheAsync<T>(CacheKeyExpiry cacheModel, Func<Task<Result<T>>> retrieveValues, bool refresh = false) where T : class;
    }
}
