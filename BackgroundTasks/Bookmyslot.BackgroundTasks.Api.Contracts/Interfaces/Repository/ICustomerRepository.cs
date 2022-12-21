using Bookmyslot.SharedKernel.ValueObject;
using System.Threading.Tasks;

namespace Bookmyslot.BackgroundTasks.Api.Contracts.Interfaces.Repository
{
    public interface ICustomerRepository
    {
        Task<Result<bool>> CreateCustomer(CustomerModel customerModel);

        Task<Result<bool>> UpdateCustomer(CustomerModel customerModel);
    }
}
