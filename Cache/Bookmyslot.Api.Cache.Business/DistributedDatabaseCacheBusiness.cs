using Bookmyslot.SharedKernel;
using Bookmyslot.SharedKernel.Contracts.Cache;
using Bookmyslot.SharedKernel.ValueObject;
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

        public async Task<Result<T>> GetFromCacheAsync<T>(
           CacheKeyExpiry cacheModel,
           Func<Task<Result<T>>> retrieveValues, bool refresh = false) where T : class
        {
            if (refresh)
            {
                return await GetInvokedMethodResponse(cacheModel, retrieveValues);
            }
            var cachedBytes = await this.distributedCache.GetAsync(cacheModel.Key);
            if (cachedBytes == null)
            {
                return await GetInvokedMethodResponse(cacheModel, retrieveValues);
            }

            var serializedResponse = Encoding.UTF8.GetString(cachedBytes);
            return new Result<T>() { Value = JsonConvert.DeserializeObject<T>(serializedResponse) };
        }

        private async Task<Result<T>> GetInvokedMethodResponse<T>(CacheKeyExpiry cacheModel, Func<Task<Result<T>>> retrieveValues) where T : class
        {
            var invokedResponse = await retrieveValues.Invoke();
            if (invokedResponse.ResultType == ResultType.Success)
            {
                var compressedBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(invokedResponse.Value));

                var options = cacheModel.IsSlidingExpiry ? new DistributedCacheEntryOptions().SetSlidingExpiration(cacheModel.ExpiryTime) :
                    new DistributedCacheEntryOptions().SetAbsoluteExpiration(cacheModel.ExpiryTime);

                await this.distributedCache.SetAsync(cacheModel.Key, compressedBytes, options);
            }

            return invokedResponse;
        }
    }
}
