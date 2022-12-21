using Bookmyslot.Api.Customers.Domain;
using Bookmyslot.SharedKernel.ValueObject;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Customers.Contracts.Interfaces
{
    public interface IProfileSettingsRepository
    {
        Task<Result<bool>> UpdateProfileSettings(ProfileSettingsModel profileSettingsModel, string customerId);

        Task<Result<ProfileSettingsModel>> GetProfileSettingsByCustomerId(string customerId);

        Task<Result<bool>> UpdateProfilePicture(string customerId, string profilePictureUrl);
    }
}
