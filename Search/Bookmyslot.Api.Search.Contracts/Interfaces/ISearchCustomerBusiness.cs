using Bookmyslot.Api.Common.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Search.Contracts.Interfaces
{
    public interface ISearchCustomerBusiness
    {
        Task<Response<List<SearchCustomerModel>>> SearchCustomers(string searchKey, PageParameterModel pageParameterModel);
    }
}
