using Bookmyslot.Api.Common;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;

namespace Bookmyslot.Api.Customers.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        public Response<CustomerModel> GetCustomer(string email)
        {
            return new Response<CustomerModel>() { Result = new CustomerModel() { Prefix = "Mr", FirstName = "Tar", LastName = "Lk", Email = "a@gmail.com" } };
        }
    }
}
