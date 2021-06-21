using Bookmyslot.Api.Common.Contracts;
using System.Threading.Tasks;

namespace Bookmyslot.BackgroundTasks.Api.Contracts.Interfaces.Business
{
    public interface ICustomerBusiness
    {
        Task<Response<bool>> CreateCustomer(CustomerModel customerModel);

        Task<Response<bool>> UpdateCustomer(CustomerModel customerModel);
    }
}
