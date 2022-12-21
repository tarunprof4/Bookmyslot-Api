using Bookmyslot.Api.Azure.Contracts.Interfaces;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.Customers.Domain;
using Bookmyslot.SharedKernel;
using Bookmyslot.SharedKernel.ValueObject;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Customers.Business
{
    public class ProfileSettingsBusiness : IProfileSettingsBusiness
    {
        private readonly IProfileSettingsRepository profileSettingsRepository;
        private readonly IBlobRepository blobRepository;

        public ProfileSettingsBusiness(IProfileSettingsRepository profileSettingsRepository, IBlobRepository blobRepository)
        {
            this.profileSettingsRepository = profileSettingsRepository;
            this.blobRepository = blobRepository;
        }

        public async Task<Result<bool>> UpdateProfileSettings(ProfileSettingsModel profileSettingsModel, string customerId)
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


        public async Task<Result<ProfileSettingsModel>> GetProfileSettingsByCustomerId(string customerId)
        {
            return await profileSettingsRepository.GetProfileSettingsByCustomerId(customerId);
        }

        public async Task<Result<string>> UpdateProfilePicture(IFormFile file, string customerId, string firstName)
        {
            var blobUpdateProfilePictureResponse = await this.blobRepository.UpdateProfilePicture(file, customerId, firstName);

            if (blobUpdateProfilePictureResponse.ResultType == ResultType.Success)
            {
                var updateProfileResponse = await this.profileSettingsRepository.UpdateProfilePicture(customerId, blobUpdateProfilePictureResponse.Value);
                if (updateProfileResponse.ResultType != ResultType.Success)
                {
                    return new Result<string>() { ResultType = updateProfileResponse.ResultType, Messages = updateProfileResponse.Messages };
                }
            }

            return blobUpdateProfilePictureResponse;
        }


    }

}
