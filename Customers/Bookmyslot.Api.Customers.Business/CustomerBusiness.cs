using Bookmyslot.Api.Common;
using Bookmyslot.Api.Customers.Business.Validations;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using FluentValidation.Results;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

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
                return new Response<bool>() { ResultType = ResultType.ValidationError, Messages = results.Errors.Select(a=>a.ErrorMessage).ToList() };
        }

        public async Task<Response<bool>> DeleteCustomer(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return new Response<bool>() { ResultType = ResultType.ValidationError, Messages = new List<string>() { Constants.EmailIdNotValid } };
            }

            var customerExists = await CheckIfCustomerExists(email);
            if (customerExists)
            {
                return await this.customerRepository.DeleteCustomer(email);
            }
            return new Response<bool>() { ResultType = ResultType.Error, Messages = new List<string>() { Constants.CustomerNotFound } };
        }

        public async Task<Response<IEnumerable<CustomerModel>>> GetAllCustomers()
        {
            return await this.customerRepository.GetAllCustomers();
        }

        public async Task<Response<CustomerModel>> GetCustomer(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return new Response<CustomerModel>() { ResultType = ResultType.ValidationError, Messages = new List<string>() { Constants.EmailIdNotValid } };
            }

            return await customerRepository.GetCustomer(email);
        }

        public async Task<Response<bool>> UpdateCustomer(CustomerModel customerModel)
        {
            var customerExists = await CheckIfCustomerExists(customerModel.Email);
            if (customerExists)
            {
                return await this.customerRepository.UpdateCustomer(customerModel);
            }

            return new Response<bool>() { ResultType = ResultType.Error, Messages = new List<string>() { Constants.CustomerNotFound } };
        }

        private async Task<bool> CheckIfCustomerExists(string email)
        {
            var customer = await customerRepository.GetCustomer(email);
            if (customer.HasResult)
                return true;

            return false;
        }
    }
}
