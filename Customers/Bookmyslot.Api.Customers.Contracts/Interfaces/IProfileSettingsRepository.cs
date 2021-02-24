

using Bookmyslot.Api.Common.Contracts;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Customers.Contracts.Interfaces
{
    public interface IProfileSettingsRepository
    {
        Task<Response<bool>> UpdateProfileSettings(ProfileSettingsModel profileSettingsModel, string customerId);

        Task<Response<ProfileSettingsModel>> GetProfileSettingsByCustomerId(string customerId);
    }
}
