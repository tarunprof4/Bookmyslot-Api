using Bookmyslot.Api.Common.Contracts;
using System.Threading.Tasks;

namespace Bookmyslot.BackgroundTasks.Api.Contracts.Interfaces.Repository
{
    public interface ICustomerRepository
    {
        Task<Response<bool>> CreateCustomer(CustomerModel customerModel);
    }
}
