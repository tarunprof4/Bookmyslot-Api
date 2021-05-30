using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.Customers.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Customers.Business
{
    public class RegisterCustomerBusiness : IRegisterCustomerBusiness
    {
        private readonly IRegisterCustomerRepository registerCustomerRepository;
        private readonly ICustomerRepository customerRepository;
        public RegisterCustomerBusiness(IRegisterCustomerRepository registerCustomerRepository, ICustomerRepository customerRepository)
        {
            this.registerCustomerRepository = registerCustomerRepository;
            this.customerRepository = customerRepository;
        }

        public async Task<Response<string>> RegisterCustomer(RegisterCustomerModel registerCustomerModel)
        {
            var customerExists = await CheckIfCustomerExists(registerCustomerModel.Email);
            if (!customerExists)
            {
                SanitizeCustomerModel(registerCustomerModel);
                return await registerCustomerRepository.RegisterCustomer(registerCustomerModel);
                
            }

            return new Response<string>() { ResultType = ResultType.ValidationError, Messages = new List<string>() { AppBusinessMessagesConstants.EmailIdExists } };
        }

        private void SanitizeCustomerModel(RegisterCustomerModel registerCustomerModel)
        {
            registerCustomerModel.FirstName = registerCustomerModel.FirstName.Trim();
            registerCustomerModel.LastName = registerCustomerModel.LastName.Trim();
            registerCustomerModel.Email = registerCustomerModel.Email.Trim().ToLowerInvariant();
        }


        private async Task<bool> CheckIfCustomerExists(string email)
        {
            var customerModelResponse = await customerRepository.GetCustomerByEmail(email);
            if (customerModelResponse.ResultType == ResultType.Success)
                return true;

            return false;
        }
    }
}
