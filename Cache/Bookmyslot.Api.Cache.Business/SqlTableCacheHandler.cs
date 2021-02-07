using Bookmyslot.Api.Cache.Contracts;
using Bookmyslot.Api.Cache.Contracts.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Cache.Business
{
    public class SqlTableCacheHandler : ITableCacheHandler
    {
        private readonly ICacheRepository cacheRepository;

        public SqlTableCacheHandler(ICacheRepository cacheRepository)
        {
            this.cacheRepository = cacheRepository;
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
                    var serialized = JsonConvert.SerializeObject(invokedResponse);
                    var cacheModel = new CacheModel()
                    {
                        Key = key,
                        Type = cacheType,
                        Value = serialized,
                        ExpiryInSeconds = expiryInSeconds
                    };
                    await this.cacheRepository.CreateCache(cacheModel);
                }

                return invokedResponse;
            }

            return new Response<T>() { Result = JsonConvert.DeserializeObject<Response<T>>(cachedResponse.Result).Result };
        }
    }
}
