using Bookmyslot.Api.Common;

namespace Bookmyslot.Api.Customers.Contracts.Interfaces
{
    public interface ICustomerRepository
    {
        Response<CustomerModel> GetCustomer(string email);
    }
}
