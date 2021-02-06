using Bookmyslot.Api.Common.Contracts;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Customers.Contracts.Interfaces
{
    public interface IRegisterCustomerRepository
    {
        Task<Response<string>> CreateCustomer(RegisterCustomerModel registerCustomerModel);
    }
}
