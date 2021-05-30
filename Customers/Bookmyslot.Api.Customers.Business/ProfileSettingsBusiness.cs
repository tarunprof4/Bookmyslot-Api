using Bookmyslot.Api.Azure.Contracts.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Microsoft.AspNetCore.Http;
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
            SanitizeProfileSettingsModel(profileSettingsModel);
            return await this.profileSettingsRepository.UpdateProfileSettings(profileSettingsModel, customerId);
        }

        private void SanitizeProfileSettingsModel(ProfileSettingsModel profileSettingsModel)
        {
            profileSettingsModel.FirstName = profileSettingsModel.FirstName.Trim();
            profileSettingsModel.LastName = profileSettingsModel.LastName.Trim();
            profileSettingsModel.Gender = profileSettingsModel.Gender.Trim();
        }
       

        public async Task<Response<ProfileSettingsModel>> GetProfileSettingsByCustomerId(string customerId)
        {
            return await profileSettingsRepository.GetProfileSettingsByCustomerId(customerId);
        }

        public async Task<Response<string>> UpdateProfilePicture(IFormFile file, string customerId, string firstName)
        {
            var blobUpdateProfilePictureResponse = await this.blobRepository.UpdateProfilePicture(file, customerId, firstName);

            if (blobUpdateProfilePictureResponse.ResultType == ResultType.Success)
            {
                var updateProfileResponse = await this.profileSettingsRepository.UpdateProfilePicture(customerId, blobUpdateProfilePictureResponse.Result);
                if (updateProfileResponse.ResultType != ResultType.Success)
                {
                    return new Response<string>() { ResultType = updateProfileResponse.ResultType, Messages = updateProfileResponse.Messages };
                }
            }

            return blobUpdateProfilePictureResponse;
        }


    }

}
