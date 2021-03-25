﻿using Bookmyslot.Api.Azure.Contracts.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Customers.Business
{
    public class ProfileSettingsBusiness : IProfileSettingsBusiness
    {
        private readonly IProfileSettingsRepository profileSettingsRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly IBlobRepository blobRepository;
        
        public ProfileSettingsBusiness(IProfileSettingsRepository profileSettingsRepository, ICustomerRepository customerRepository, IBlobRepository blobRepository)
        {
            this.profileSettingsRepository = profileSettingsRepository;
            this.customerRepository = customerRepository;
            this.blobRepository = blobRepository;
        }

        public async Task<Response<bool>> UpdateProfileSettings(ProfileSettingsModel profileSettingsModel, string customerId)
        {
            var profileExists = await CheckIfProfileExists(customerId);
            if (profileExists)
            {
                SanitizeProfileSettingsModel(profileSettingsModel);
                return await this.profileSettingsRepository.UpdateProfileSettings(profileSettingsModel, customerId);
            }

            return new Response<bool>() { ResultType = ResultType.Empty, Messages = new List<string>() { AppBusinessMessagesConstants.CustomerNotFound } };
        }

        private void SanitizeProfileSettingsModel(ProfileSettingsModel profileSettingsModel)
        {
            profileSettingsModel.FirstName = profileSettingsModel.FirstName.Trim();
            profileSettingsModel.LastName = profileSettingsModel.LastName.Trim();
            profileSettingsModel.Gender = profileSettingsModel.Gender.Trim();
        }


        private async Task<bool> CheckIfProfileExists(string customerId)
        {
            var customerModelResponse = await customerRepository.GetCustomerById(customerId);
            if (customerModelResponse.ResultType == ResultType.Success)
                return true;

            return false;
        }

        public async Task<Response<ProfileSettingsModel>> GetProfileSettingsByCustomerId(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                return new Response<ProfileSettingsModel>() { ResultType = ResultType.ValidationError, Messages = new List<string>() { AppBusinessMessagesConstants.CustomerIdNotValid } };
            }

            return await profileSettingsRepository.GetProfileSettingsByCustomerId(customerId);
        }

        public async Task<Response<string>> UpdateProfilePicture(IFormFile file, string customerId)
        {
            return await this.blobRepository.SaveProfilePicture(file, customerId);
        }

      
    }

}
