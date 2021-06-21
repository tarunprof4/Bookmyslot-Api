using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Search.Contracts;
using System.Threading.Tasks;

namespace Bookmyslot.BackgroundTasks.Api.Contracts.Interfaces.Repository
{
    public interface ICustomerRepository
    {
        Task<Response<bool>> CreateSearchCustomer(SearchCustomerModel searchCustomerModel);

        Task<Response<bool>> UpdateSearchCustomer(SearchCustomerModel searchCustomerModel);
    }
}
