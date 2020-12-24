using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Customers.Business.Validations;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using FluentValidation.Results;
using System.Collections.Generic;
using System.Linq;
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

        

        public async Task<Response<string>> CreateCustomer(CustomerModel customerModel)
        {
            var validator = new CustomerValidator();
            ValidationResult results = validator.Validate(customerModel);

            if (results.IsValid)
            {
                SanitizeCustomerModel(customerModel);
                return await customerRepository.CreateCustomer(customerModel);
            }
                

            else
                return new Response<string>() { ResultType = ResultType.ValidationError, Messages = results.Errors.Select(a => a.ErrorMessage).ToList() };
        }

        public async Task<Response<bool>> DeleteCustomer(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return new Response<bool>() { ResultType = ResultType.ValidationError, Messages = new List<string>() { AppBusinessMessages.EmailIdNotValid } };
            }

            var customerExists = await CheckIfCustomerExists(email);
            if (customerExists)
            {
                return await this.customerRepository.DeleteCustomer(email);
            }
            return new Response<bool>() { ResultType = ResultType.Error, Messages = new List<string>() { AppBusinessMessages.CustomerNotFound } };
        }

        public async Task<Response<IEnumerable<CustomerModel>>> GetAllCustomers()
        {
            return await this.customerRepository.GetAllCustomers();
        }

        public async Task<Response<CustomerModel>> GetCustomer(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return new Response<CustomerModel>() { ResultType = ResultType.ValidationError, Messages = new List<string>() { AppBusinessMessages.EmailIdNotValid } };
            }

            return await customerRepository.GetCustomer(email);
        }

        public async Task<Response<bool>> UpdateCustomer(CustomerModel customerModel)
        {
            var validator = new CustomerValidator();
            ValidationResult results = validator.Validate(customerModel);

            if (results.IsValid)
            {
                SanitizeCustomerModel(customerModel);
                var customerExists = await CheckIfCustomerExists(customerModel.Email);
                if (customerExists)
                {
                    return await this.customerRepository.UpdateCustomer(customerModel);
                }

                return new Response<bool>() { ResultType = ResultType.Error, Messages = new List<string>() { AppBusinessMessages.CustomerNotFound } };
            }

            else
                return new Response<bool>() { ResultType = ResultType.ValidationError, Messages = results.Errors.Select(a => a.ErrorMessage).ToList() };
        }

        private async Task<bool> CheckIfCustomerExists(string email)
        {
            var customer = await customerRepository.GetCustomer(email);
            if (customer.HasResult)
                return true;

            return false;
        }

        private void SanitizeCustomerModel(CustomerModel customerModel)
        {
            customerModel.FirstName = customerModel.FirstName.Trim();
            customerModel.MiddleName = customerModel.MiddleName.Trim();
            customerModel.LastName = customerModel.LastName.Trim();
            customerModel.Gender = customerModel.Gender.Trim();
            customerModel.Email = customerModel.Email.Trim();
        }
    }
}
