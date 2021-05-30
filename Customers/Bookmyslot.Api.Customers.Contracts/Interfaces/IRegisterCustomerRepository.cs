using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Customers.Domain;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Customers.Contracts.Interfaces
{
    public interface IRegisterCustomerRepository
    {
        Task<Response<string>> RegisterCustomer(RegisterCustomerModel registerCustomerModel);
    }
}
