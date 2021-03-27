using Bookmyslot.Api.Common.Contracts;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Search.Contracts.Interfaces
{
    public interface ISearchRepository
    {
        Task<Response<T>> GetPreProcessedSearchedResponse<T>(string searchType, string searchKey);

        Task<Response<bool>> SavePreProcessedSearchedResponse<T>(string searchType, string searchKey, T response);
    }
}
