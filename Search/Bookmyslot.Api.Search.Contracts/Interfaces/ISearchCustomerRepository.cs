using Bookmyslot.Api.Common.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Search.Contracts.Interfaces
{
    public interface ISearchCustomerRepository
    {
        Task<Response<SearchCustomerModel>> SearchCustomersByUserName(string userName);
        Task<Response<List<SearchCustomerModel>>> SearchCustomersByName(string name);
        Task<Response<List<SearchCustomerModel>>> SearchCustomersByBioHeadLine(string bioHeadLine);
    }
}
