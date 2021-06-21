using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Search.Contracts;
using System.Threading.Tasks;

namespace Bookmyslot.BackgroundTasks.Api.Contracts.Interfaces.Business
{
    public interface ICustomerBusiness
    {
        Task<Response<bool>> CreateCustomer(SearchCustomerModel searchCustomerModel);

        Task<Response<bool>> UpdateCustomer(SearchCustomerModel searchCustomerModel);
    }
}
