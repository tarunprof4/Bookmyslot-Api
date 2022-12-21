using Bookmyslot.Api.Customers.Domain;
using Bookmyslot.SharedKernel.ValueObject;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Customers.Contracts.Interfaces
{
    public interface IProfileSettingsBusiness
    {
        Task<Result<ProfileSettingsModel>> GetProfileSettingsByCustomerId(string customerId);
        Task<Result<bool>> UpdateProfileSettings(ProfileSettingsModel profileSettingsModel, string customerId);
        Task<Result<string>> UpdateProfilePicture(IFormFile file, string customerId, string firstName);
    }
}
