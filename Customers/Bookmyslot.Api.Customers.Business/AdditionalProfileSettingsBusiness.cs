using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Customers.Business
{
    public class AdditionalProfileSettingsBusiness : IAdditionalProfileSettingsBusiness
    {
        private readonly IAdditionalProfileSettingsRepository additionalProfileSettingsRepository;
        private readonly ICustomerRepository customerRepository;

        public AdditionalProfileSettingsBusiness(IAdditionalProfileSettingsRepository additionalProfileSettingsRepository, ICustomerRepository customerRepository)
        {
            this.additionalProfileSettingsRepository = additionalProfileSettingsRepository;
            this.customerRepository = customerRepository;
        }


        public async Task<Response<AdditionalProfileSettingsModel>> GetAdditionalProfileSettingsByCustomerId(string customerId)
        {
            return await this.additionalProfileSettingsRepository.GetAdditionalProfileSettingsByCustomerId(customerId);
        }

        public async Task<Response<bool>> UpdateAdditionalProfileSettings(string customerId, AdditionalProfileSettingsModel additionalProfileSettingsModel)
        {
            SanitizeAdditionalProfileSettingsModel(additionalProfileSettingsModel);
            return await this.additionalProfileSettingsRepository.UpdateAdditionalProfileSettings(customerId, additionalProfileSettingsModel);
        }


        private void SanitizeAdditionalProfileSettingsModel(AdditionalProfileSettingsModel additionalProfileSettingsModel)
        {
            additionalProfileSettingsModel.BioHeadLine = additionalProfileSettingsModel.BioHeadLine.Trim();
        }
    }
}
