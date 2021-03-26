using Bookmyslot.Api.Common.Contracts;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Customers.Contracts.Interfaces
{
    public interface IAdditionalProfileSettingsRepository
    {
        Task<Response<AdditionalProfileSettingsModel>> GetAdditionalProfileSettingsByCustomerId(string customerId);
        Task<Response<bool>> UpdateAdditionalProfileSettings(string customerId, AdditionalProfileSettingsModel additionalProfileSettingsModel);
    }
}
