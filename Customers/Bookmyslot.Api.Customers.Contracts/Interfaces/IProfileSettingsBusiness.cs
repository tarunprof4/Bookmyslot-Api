using Bookmyslot.Api.Common.Contracts;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Customers.Contracts.Interfaces
{
    public interface IProfileSettingsBusiness
    {

        Task<Response<ProfileSettingsModel>> GetProfileSettingsByCustomerId(string customerId);
        Task<Response<bool>> UpdateProfileSettings(ProfileSettingsModel profileSettingsModel, string customerId);

        Task<Response<string>> UpdateProfilePicture(IFormFile file, string customerId, string firstName);

        


    }
}
