using Bookmyslot.Api.Common.Contracts;
using System;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Cache.Contracts.Interfaces
{

    public interface IDistributedInMemoryCacheBuisness
    {
        Task<Response<T>> GetFromCacheAsync<T>(CacheModel cacheModel, Func<Task<Response<T>>> retrieveValues, bool refresh = false) where T : class;
    }
}
