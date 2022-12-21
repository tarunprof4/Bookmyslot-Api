using Bookmyslot.Api.Customers.Domain;
using Bookmyslot.SharedKernel.ValueObject;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Customers.Contracts.Interfaces
{
    public interface ICustomerSettingsBusiness
    {
        Task<Result<CustomerSettingsModel>> GetCustomerSettings(string customerId);
        Task<Result<bool>> UpdateCustomerSettings(string customerId, CustomerSettingsModel customerSettingsModel);
    }
}
