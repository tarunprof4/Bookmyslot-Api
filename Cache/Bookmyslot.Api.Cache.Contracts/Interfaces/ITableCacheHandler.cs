using Bookmyslot.Api.Common.Contracts;
using System;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Cache.Contracts.Interfaces
{

    public interface ITableCacheHandler
    {
        Task<Response<T>> GetFromCacheAsync<T>(string key, Func<Task<Response<T>>> retrieveValues, int expiryInSeconds) where T : class;
    }
}
