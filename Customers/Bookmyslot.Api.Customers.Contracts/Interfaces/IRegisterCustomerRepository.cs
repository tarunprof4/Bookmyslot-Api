using Bookmyslot.Api.Customers.Domain;
using Bookmyslot.SharedKernel.ValueObject;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Customers.Contracts.Interfaces
{
    public interface IRegisterCustomerRepository
    {
        Task<Result<string>> RegisterCustomer(RegisterCustomerModel registerCustomerModel);
    }
}
