using Bookmyslot.Api.Authentication.Common;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
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

        public async Task<Response<CustomerModel>> GetCustomerByEmail(string email)
        {
            email = email.ToLowerInvariant();
            return await customerRepository.GetCustomerByEmail(email);
        }

        public async Task<Response<string>> GetCustomerIdByEmail(string email)
        {
            email = email.ToLowerInvariant();
            return await customerRepository.GetCustomerIdByEmail(email);
        }



        public async Task<Response<CustomerModel>> GetCustomerById(string customerId)
        {
            return await customerRepository.GetCustomerById(customerId);
        }



        public async Task<Response<List<CustomerModel>>> GetCustomersByCustomerIds(IEnumerable<string> customerIds)
        {
            customerIds = customerIds.Distinct();
            return await this.customerRepository.GetCustomersByCustomerIds(customerIds);
        }

        public async Task<Response<CurrentUserModel>> GetCurrentUserByEmail(string email)
        {
            var customerModelResponse = await this.customerRepository.GetCustomerByEmail(email);
            return new Response<CurrentUserModel>() { Result = CreateCurrentUserModel(customerModelResponse.Result) };
        }

        private CurrentUserModel CreateCurrentUserModel(CustomerModel customerModel)
        {
            return new CurrentUserModel()
            {
                Id = customerModel.Id,
                FirstName = customerModel.FirstName,
                LastName = customerModel.LastName,
                BioHeadLine = customerModel.BioHeadLine,
                IsVerified = customerModel.IsVerified,
                ProfilePictureUrl = customerModel.ProfilePictureUrl,
                UserName = customerModel.UserName,
                Email = customerModel.Email
            };
        }

    }
}
