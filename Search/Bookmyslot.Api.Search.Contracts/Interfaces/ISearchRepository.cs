using Bookmyslot.SharedKernel.ValueObject;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Search.Contracts.Interfaces
{
    public interface ISearchRepository
    {
        Task<Result<T>> GetPreProcessedSearchedResponse<T>(string searchType, string searchKey);

        Task<Result<bool>> SavePreProcessedSearchedResponse<T>(string searchType, string searchKey, T response);
    }
}
