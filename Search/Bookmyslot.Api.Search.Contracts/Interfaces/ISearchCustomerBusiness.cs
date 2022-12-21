using Bookmyslot.SharedKernel.ValueObject;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Search.Contracts.Interfaces
{
    public interface ISearchCustomerBusiness
    {
        Task<Result<List<SearchCustomerModel>>> SearchCustomers(string searchKey, PageParameter pageParameterModel);
    }
}
