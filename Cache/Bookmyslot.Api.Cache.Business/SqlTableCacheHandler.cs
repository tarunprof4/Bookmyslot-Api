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
            var retrievedResponse = await this.cacheRepository.GetCache(cacheType, key);

            if (retrievedResponse.ResultType == ResultType.Empty)
            {
                var result = await retrieveValues.Invoke();
                if (string.IsNullOrWhiteSpace(retrievedResponse.Result))
                {
                    var serialized = JsonConvert.SerializeObject(result);
                    var cacheModel = new CacheModel()
                    {
                        Key = key,
                        Type = cacheType,
                        Value = serialized,
                        ExpiryInSeconds = expiryInSeconds
                    };
                    await this.cacheRepository.CreateCache(cacheModel);
                }

                return result;
            }

            return new Response<T>() { Result = JsonConvert.DeserializeObject<Response<T>>(retrievedResponse.Result).Result };
        }
    }
}
