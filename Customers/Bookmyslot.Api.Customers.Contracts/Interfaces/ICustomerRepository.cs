using Bookmyslot.Api.Common;
using System.Collections.Generic;

namespace Bookmyslot.Api.Customers.Contracts.Interfaces
{
    public interface ICustomerRepository
    {
        Response<IEnumerable<CustomerModel>> GetAllCustomers();
        Response<CustomerModel> GetCustomer(string email);

        Response<bool> CreateCustomer(CustomerModel customerModel);

        Response<bool> UpdateCustomer(CustomerModel customerModel);

        Response<bool> DeleteCustomer(string email);
    }
}
