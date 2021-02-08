using Bookmyslot.Api.Common.Compression.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Cache.Contracts.Interfaces
{

    public class DistributedInMemoryCacheBuisness : IDistributedInMemoryCacheBuisness
    {
        private readonly IDistributedCache distributedCache;
        private readonly ICompression compression;

        public DistributedInMemoryCacheBuisness(IDistributedCache distributedCache, ICompression compression)
        {
            this.distributedCache = distributedCache;
            this.compression = compression;
        }
        public async Task<Response<T>> GetFromCacheAsync<T>(CacheModel cacheModel, Func<Task<Response<T>>> retrieveValues) where T : class
        {
            var cachedResponse = await this.distributedCache.GetStringAsync(cacheModel.Key);

            if (string.IsNullOrWhiteSpace(cachedResponse))
            {
                var invokedResponse = await retrieveValues.Invoke();
                if (invokedResponse.ResultType == ResultType.Success)
                {
                    var compressedResponse = compression.Compress(invokedResponse.Result);

                    var options = cacheModel.IsSlidingExpiry ? new DistributedCacheEntryOptions().SetSlidingExpiration(cacheModel.ExpiryTimeUtc) :
                        new DistributedCacheEntryOptions().SetAbsoluteExpiration(cacheModel.ExpiryTimeUtc);

                    await this.distributedCache.SetStringAsync(cacheModel.Key, compressedResponse, options);
                }

                return invokedResponse;
            }

            var deCompressedResponse = compression.Decompress<T>(cachedResponse);
            return new Response<T>() { Result = deCompressedResponse };
        }

    
    }
}
