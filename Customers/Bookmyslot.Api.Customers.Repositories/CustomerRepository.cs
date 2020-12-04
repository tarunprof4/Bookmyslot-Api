using Bookmyslot.Api.Common;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;

namespace Bookmyslot.Api.Customers.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        public Response<Customer> GetCustomer(string email)
        {
            return new Response<Customer>() { Result = new Customer() { Prefix = "Mr", FirstName = "Tar", LastName = "Lk", Email = "a@gmail.com" } };
        }
    }
}
