using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Customers.Business.Validations;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using FluentValidation.Results;
using System;
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
                var customerExists = await CheckIfCustomerExists(customerModel.Email);
                if (!customerExists.Item1)
                {
                    SanitizeCustomerModel(customerModel);
                    return await customerRepository.CreateCustomer(customerModel);
                }

                return new Response<string>() { ResultType = ResultType.Error, Messages = new List<string>() { AppBusinessMessages.EmailIdExists } };
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
            if (customerExists.Item1)
            {
                return await this.customerRepository.DeleteCustomer(email);
            }
            return Response<bool>.Empty(new List<string>() { AppBusinessMessages.CustomerNotFound });
        }

        public async Task<Response<IEnumerable<CustomerModel>>> GetAllCustomers()
        {
            return await this.customerRepository.GetAllCustomers();
        }

       

        public async Task<Response<CustomerModel>> GetCustomerByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return new Response<CustomerModel>() { ResultType = ResultType.ValidationError, Messages = new List<string>() { AppBusinessMessages.EmailIdNotValid } };
            }

            return await customerRepository.GetCustomerByEmail(email);
        }

        public async Task<Response<CustomerModel>> GetCustomerById(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                return new Response<CustomerModel>() { ResultType = ResultType.ValidationError, Messages = new List<string>() { AppBusinessMessages.CustomerIdNotValid } };
            }

            return await customerRepository.GetCustomerById(customerId);
        }

        public async Task<Response<List<CustomerModel>>> GetCustomersByCustomerIds(IEnumerable<string> customerIds)
        {
            return await this.customerRepository.GetCustomersByCustomerIds(customerIds);
        }

        public async Task<Response<bool>> UpdateCustomer(CustomerModel customerModel)
        {
            var validator = new CustomerValidator();
            ValidationResult results = validator.Validate(customerModel);

            if (results.IsValid)
            {
                var customerExists = await CheckIfCustomerExists(customerModel.Email);
                if (customerExists.Item1)
                {
                    SanitizeCustomerModel(customerModel);
                    customerModel.Id = customerExists.Item2.Id;

                    return await this.customerRepository.UpdateCustomer(customerModel);
                }

                return new Response<bool>() { ResultType = ResultType.Empty, Messages = new List<string>() { AppBusinessMessages.CustomerNotFound } };
            }

            else
                return new Response<bool>() { ResultType = ResultType.ValidationError, Messages = results.Errors.Select(a => a.ErrorMessage).ToList() };
        }

        private async Task<Tuple<bool, CustomerModel>> CheckIfCustomerExists(string email)
        {
            var customerModelResponse = await customerRepository.GetCustomerByEmail(email);
            if (customerModelResponse.ResultType == ResultType.Success)
                return new Tuple<bool, CustomerModel>(true, customerModelResponse.Result);

            return new Tuple<bool, CustomerModel>(false, customerModelResponse.Result);
        }

        private void SanitizeCustomerModel(CustomerModel customerModel)
        {
            customerModel.FirstName = customerModel.FirstName.Trim();
            customerModel.LastName = customerModel.LastName.Trim();
            customerModel.Gender = customerModel.Gender.Trim();
            customerModel.Email = customerModel.Email.Trim();
        }
    }
}
