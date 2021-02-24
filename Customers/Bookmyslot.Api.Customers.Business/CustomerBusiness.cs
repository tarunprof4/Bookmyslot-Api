using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
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

        public async Task<Response<CustomerModel>> GetCustomerByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return new Response<CustomerModel>() { ResultType = ResultType.ValidationError, Messages = new List<string>() { AppBusinessMessagesConstants.EmailIdNotValid } };
            }

            email = email.ToLowerInvariant();
            return await customerRepository.GetCustomerByEmail(email);
        }

        public async Task<Response<string>> GetCustomerIdByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return new Response<string>() { ResultType = ResultType.ValidationError, Messages = new List<string>() { AppBusinessMessagesConstants.EmailIdNotValid } };
            }

            email = email.ToLowerInvariant();
            return await customerRepository.GetCustomerIdByEmail(email);
        }

        public async Task<Response<CustomerModel>> GetCustomerById(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                return new Response<CustomerModel>() { ResultType = ResultType.ValidationError, Messages = new List<string>() { AppBusinessMessagesConstants.CustomerIdNotValid } };
            }

            return await customerRepository.GetCustomerById(customerId);
        }

      

        public async Task<Response<List<CustomerModel>>> GetCustomersByCustomerIds(IEnumerable<string> customerIds)
        {
            return await this.customerRepository.GetCustomersByCustomerIds(customerIds);
        }
    }
}
