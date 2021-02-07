using Bookmyslot.Api.Common.Contracts;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Cache.Contracts.Interfaces
{

    public interface ICacheRepository
    {
        Task<Response<bool>> CreateCache(CacheModel cacheModel);

        Task<Response<string>> GetCache(string cacheType, string cacheKey);
    }
}
