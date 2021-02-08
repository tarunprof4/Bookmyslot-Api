using Bookmyslot.Api.Cache.Contracts;
using Bookmyslot.Api.Cache.Contracts.Interfaces;
using Bookmyslot.Api.Common.Compression.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using System;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Cache.Business
{
    public class DatabaseCacheBusiness : IDatabaseCacheBuisness
    {
        private readonly ICacheRepository cacheRepository;
        private readonly ICompression compression;

        public DatabaseCacheBusiness(ICacheRepository cacheRepository, ICompression compression)
        {
            this.cacheRepository = cacheRepository;
            this.compression = compression;
        }

        public async Task<Response<T>> GetFromCacheAsync<T>(
           string key,
           Func<Task<Response<T>>> retrieveValues,
           int expiryInSeconds) where T : class
        {
            var cacheType = typeof(T).ToString();
            var cachedResponse = await this.cacheRepository.GetCache(cacheType, key);

            if (cachedResponse.ResultType == ResultType.Empty)
            {
                var invokedResponse = await retrieveValues.Invoke();
                if (invokedResponse.ResultType == ResultType.Success)
                {
                    var compressedResponse = compression.Compress(invokedResponse.Result);
                    var cacheModel = new CacheModel()
                    {
                        Key = key,
                        Type = cacheType,
                        Value = compressedResponse,
                        ExpiryTimeUtc = new TimeSpan(0, 0, expiryInSeconds)
                    };
                    await this.cacheRepository.CreateCache(cacheModel);
                }

                return invokedResponse;
            }

            var deCompressedResponse = compression.Decompress<T>(cachedResponse.Result);
            return new Response<T>() { Result = deCompressedResponse };
        }
    }
}
