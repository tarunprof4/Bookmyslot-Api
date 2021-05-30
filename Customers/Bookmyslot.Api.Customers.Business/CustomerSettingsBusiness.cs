using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.Customers.Domain;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Customers.Business
{
    public class CustomerSettingsBusiness : ICustomerSettingsBusiness
    {
        private readonly ICustomerSettingsRepository customerAdditionalInformationRepository;
        public CustomerSettingsBusiness(ICustomerSettingsRepository customerAdditionalInformationRepository)
        {
            this.customerAdditionalInformationRepository = customerAdditionalInformationRepository;
        }

        public async Task<Response<CustomerSettingsModel>> GetCustomerSettings(string customerId)
        {
            return await this.customerAdditionalInformationRepository.GetCustomerSettings(customerId);
        }

        public async Task<Response<bool>> UpdateCustomerSettings(string customerId, CustomerSettingsModel customerSettingsModel)
        {
            return await this.customerAdditionalInformationRepository.UpdateCustomerSettings(customerId, customerSettingsModel);
        }
    }
}
