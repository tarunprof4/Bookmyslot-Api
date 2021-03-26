using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
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
            if (string.IsNullOrWhiteSpace(customerId))
            {
                return new Response<AdditionalProfileSettingsModel>() { ResultType = ResultType.ValidationError, Messages = new List<string>() { AppBusinessMessagesConstants.CustomerIdNotValid } };
            }

            return await this.additionalProfileSettingsRepository.GetAdditionalProfileSettingsByCustomerId(customerId);
        }

        public async Task<Response<bool>> UpdateAdditionalProfileSettings(string customerId, AdditionalProfileSettingsModel additionalProfileSettingsModel)
        {
            var profileExists = await CheckIfProfileExists(customerId);
            if (profileExists)
            {
                SanitizeAdditionalProfileSettingsModel(additionalProfileSettingsModel);
                return await this.additionalProfileSettingsRepository.UpdateAdditionalProfileSettings(customerId, additionalProfileSettingsModel);
            }

            return new Response<bool>() { ResultType = ResultType.Empty, Messages = new List<string>() { AppBusinessMessagesConstants.CustomerNotFound } };
        }


        private void SanitizeAdditionalProfileSettingsModel(AdditionalProfileSettingsModel additionalProfileSettingsModel)
        {
            additionalProfileSettingsModel.BioHeadLine = additionalProfileSettingsModel.BioHeadLine.Trim();
        }


        private async Task<bool> CheckIfProfileExists(string customerId)
        {
            var customerModelResponse = await customerRepository.GetCustomerById(customerId);
            if (customerModelResponse.ResultType == ResultType.Success)
                return true;

            return false;
        }
    }
}
