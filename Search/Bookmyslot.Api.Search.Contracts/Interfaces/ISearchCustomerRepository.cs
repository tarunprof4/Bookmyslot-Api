using Bookmyslot.Api.Common.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Search.Contracts.Interfaces
{
    public interface ISearchCustomerRepository
    {
        Task<Response<bool>> SavePreProcessedSearchedCustomers(string searchKey, List<SearchCustomerModel> searchCustomerModels);

        Task<Response<List<SearchCustomerModel>>> GetPreProcessedSearchedCustomers(string searchKey);

        Task<Response<List<SearchCustomerModel>>> SearchCustomers(string searchKey);

        
    }
}
