using Bookmyslot.Api.Cache.Contracts;
using Bookmyslot.Api.Cache.Contracts.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Cache.Business
{
    public class DistributedDatabaseCacheBusiness : IDistributedDatabaseCacheBuisness
    {
        private readonly IDistributedCache distributedCache;

        public DistributedDatabaseCacheBusiness(IDistributedCache distributedCache)
        {
            this.distributedCache = distributedCache;
        }

        public async Task<Response<T>> GetFromCacheAsync<T>(
           CacheModel cacheModel,
           Func<Task<Response<T>>> retrieveValues) where T : class
        {
            var cachedBytes = await this.distributedCache.GetAsync(cacheModel.Key);

            if (cachedBytes == null)
            {
                var invokedResponse = await retrieveValues.Invoke();
                if (invokedResponse.ResultType == ResultType.Success)
                {
                    var compressedBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(invokedResponse.Result));

                    var options = cacheModel.IsSlidingExpiry ? new DistributedCacheEntryOptions().SetSlidingExpiration(cacheModel.ExpiryTime) :
                        new DistributedCacheEntryOptions().SetAbsoluteExpiration(cacheModel.ExpiryTime);

                    await this.distributedCache.SetAsync(cacheModel.Key, compressedBytes, options);
                }

                return invokedResponse;
            }

            var serializedResponse = Encoding.UTF8.GetString(cachedBytes);
            return new Response<T>() { Result = JsonConvert.DeserializeObject<T>(serializedResponse) };
        }
    }
}
