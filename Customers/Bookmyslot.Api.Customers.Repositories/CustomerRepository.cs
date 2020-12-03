using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;

namespace Bookmyslot.Api.Customers.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        public Customer GetCustomer(string email)
        {
            return new Customer() { Prefix = "Mr", FirstName = "Tar", LastName = "Lk", Email = "a@gmail.com" };
        }
    }
}
