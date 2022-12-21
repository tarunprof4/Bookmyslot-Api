using Bookmyslot.SharedKernel.ValueObject;
using System.Threading.Tasks;

namespace Bookmyslot.BackgroundTasks.Api.Contracts.Interfaces.Business
{
    public interface ICustomerBusiness
    {
        Task<Result<bool>> CreateCustomer(CustomerModel customerModel);

        Task<Result<bool>> UpdateCustomer(CustomerModel customerModel);
    }
}
