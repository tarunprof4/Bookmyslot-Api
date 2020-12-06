using Bookmyslot.Api.Common;
using Bookmyslot.Api.Customers.Business.Validations;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using FluentValidation.Results;
using Serilog;
using System.Collections.Generic;

namespace Bookmyslot.Api.Customers.Business
{
    public class CustomerBusiness : ICustomerBusiness
    {
        private readonly ICustomerRepository customerRepository;
        public CustomerBusiness(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        public Response<bool> CreateCustomer(CustomerModel customerModel)
        {
            var validator = new CustomerValidator();
            ValidationResult results = validator.Validate(customerModel);

            if (results.IsValid)
                return customerRepository.CreateCustomer(customerModel);

            else
                return new Response<bool>() { ResultType = ResultType.ValidationError };
        }

        public Response<bool> DeleteCustomer(string email)
        {
            return this.customerRepository.DeleteCustomer(email);
        }

        public Response<IEnumerable<CustomerModel>> GetAllCustomers()
        {
            return this.customerRepository.GetAllCustomers();
        }

        public Response<CustomerModel> GetCustomer(string email)
        {

            return customerRepository.GetCustomer(email);
        }

        public Response<bool> UpdateCustomer(CustomerModel customerModel)
        {
            return this.customerRepository.UpdateCustomer(customerModel);
        }
    }
}
