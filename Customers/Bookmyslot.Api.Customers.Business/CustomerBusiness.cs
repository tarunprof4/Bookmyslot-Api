using Bookmyslot.Api.Customers.Contracts;
using System;
using Bookmyslot.Api.Customers.Contracts.Interfaces;

namespace Bookmyslot.Api.Customers.Business
{
    public class CustomerBusiness : ICustomerBusiness
    {
        private readonly ICustomerRepository customerRepository;
        public CustomerBusiness(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }
        public Customer GetCustomer(string email)
        {
            return customerRepository.GetCustomer(email);
        }
    }
}
