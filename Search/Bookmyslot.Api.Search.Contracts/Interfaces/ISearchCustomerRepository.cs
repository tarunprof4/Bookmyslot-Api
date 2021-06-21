using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Search.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Search.Contracts.Interfaces
{
    public interface ISearchCustomerRepository
    {
        Task<Response<SearchCustomerModel>> SearchCustomersByUserName(string userName);
        Task<Response<List<SearchCustomerModel>>> SearchCustomersByName(string name, PageParameterModel pageParameterModel);
        Task<Response<List<SearchCustomerModel>>> SearchCustomersByBioHeadLine(string bioHeadLine);
    }
}
