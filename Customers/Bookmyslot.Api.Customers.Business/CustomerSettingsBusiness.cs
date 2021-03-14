using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.Location.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Customers.Business
{
    public class CustomerSettingsBusiness : ICustomerSettingsBusiness
    {
        private readonly ICustomerSettingsRepository customerAdditionalInformationRepository;
        private readonly INodaTimeZoneLocationBusiness nodaTimeZoneLocationBusiness;
        public CustomerSettingsBusiness(ICustomerSettingsRepository customerAdditionalInformationRepository, INodaTimeZoneLocationBusiness nodaTimeZoneLocationBusiness)
        {
            this.customerAdditionalInformationRepository = customerAdditionalInformationRepository;
            this.nodaTimeZoneLocationBusiness = nodaTimeZoneLocationBusiness;
        }

        public async Task<Response<CustomerSettingsModel>> GetCustomerSettings(string customerId)
        {
            return await this.customerAdditionalInformationRepository.GetCustomerSettings(customerId);
        }

        public async Task<Response<bool>> UpdateCustomerSettings(string customerId, CustomerSettingsModel customerSettingsModel)
        {
            return await this.customerAdditionalInformationRepository.UpdateCustomerSettings(customerId, customerSettingsModel);
        }
    }
}
