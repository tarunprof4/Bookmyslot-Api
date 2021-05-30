using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Customers.Domain;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Customers.Contracts.Interfaces
{
    public interface ICustomerSettingsBusiness
    {
        Task<Response<CustomerSettingsModel>> GetCustomerSettings(string customerId);
        Task<Response<bool>> UpdateCustomerSettings(string customerId, CustomerSettingsModel customerSettingsModel);
    }
}
