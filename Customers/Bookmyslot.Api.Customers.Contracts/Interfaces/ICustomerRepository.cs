using Bookmyslot.Api.Common;

namespace Bookmyslot.Api.Customers.Contracts.Interfaces
{
    public interface ICustomerRepository
    {
        Response<Customer> GetCustomer(string email);
    }
}
