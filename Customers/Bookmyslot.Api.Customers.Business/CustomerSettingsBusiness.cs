using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.Customers.Domain;
using Bookmyslot.SharedKernel.ValueObject;
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

        public async Task<Result<CustomerSettingsModel>> GetCustomerSettings(string customerId)
        {
            return await this.customerAdditionalInformationRepository.GetCustomerSettings(customerId);
        }

        public async Task<Result<bool>> UpdateCustomerSettings(string customerId, CustomerSettingsModel customerSettingsModel)
        {
            return await this.customerAdditionalInformationRepository.UpdateCustomerSettings(customerId, customerSettingsModel);
        }
    }
}
