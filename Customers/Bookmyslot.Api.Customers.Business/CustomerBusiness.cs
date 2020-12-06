using Bookmyslot.Api.Common;
using Bookmyslot.Api.Customers.Business.Validations;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using FluentValidation.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Customers.Business
{
    public class CustomerBusiness : ICustomerBusiness
    {
        private readonly ICustomerRepository customerRepository;
        public CustomerBusiness(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        public async Task<Response<bool>> CreateCustomer(CustomerModel customerModel)
        {
            var validator = new CustomerValidator();
            ValidationResult results = validator.Validate(customerModel);

            if (results.IsValid)
                return await customerRepository.CreateCustomer(customerModel);
            
            else
                return new Response<bool>() { ResultType = ResultType.ValidationError };
        }

        public async Task<Response<bool>> DeleteCustomer(string email)
        {
            return await this.customerRepository.DeleteCustomer(email);
        }

        public async Task<Response<IEnumerable<CustomerModel>>> GetAllCustomers()
        {
            return await this.customerRepository.GetAllCustomers();
        }

        public async Task<Response<CustomerModel>> GetCustomer(string email)
        {

            return await customerRepository.GetCustomer(email);
        }

        public async Task<Response<bool>> UpdateCustomer(CustomerModel customerModel)
        {
            return await this.customerRepository.UpdateCustomer(customerModel);
        }
    }
}
