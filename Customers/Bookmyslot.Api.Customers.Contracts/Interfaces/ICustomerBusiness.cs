using Bookmyslot.Api.Common;

namespace Bookmyslot.Api.Customers.Contracts.Interfaces
{
    public interface ICustomerBusiness
    {
        Response<CustomerModel> GetCustomer(string email);
    }
}
