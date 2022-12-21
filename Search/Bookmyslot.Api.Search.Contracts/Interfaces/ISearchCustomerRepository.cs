using Bookmyslot.SharedKernel.ValueObject;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Search.Contracts.Interfaces
{
    public interface ISearchCustomerRepository
    {
        Task<Result<SearchCustomerModel>> SearchCustomersByUserName(string userName);
        Task<Result<List<SearchCustomerModel>>> SearchCustomersByName(string name, PageParameter pageParameterModel);
        Task<Result<List<SearchCustomerModel>>> SearchCustomersByBioHeadLine(string bioHeadLine, PageParameter pageParameterModel);
    }
}
