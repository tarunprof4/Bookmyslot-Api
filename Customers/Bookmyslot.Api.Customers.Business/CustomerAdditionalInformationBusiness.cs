using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.Location.Contracts.Configuration;
using Bookmyslot.Api.Location.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Customers.Business
{
    public class CustomerAdditionalInformationBusiness : ICustomerAdditionalInformationBusiness
    {
        private readonly ICustomerAdditionalInformationRepository customerAdditionalInformationRepository;
        private readonly INodaTimeZoneLocationBusiness nodaTimeZoneLocationBusiness;
        public CustomerAdditionalInformationBusiness(ICustomerAdditionalInformationRepository customerAdditionalInformationRepository, INodaTimeZoneLocationBusiness nodaTimeZoneLocationBusiness)
        {
            this.customerAdditionalInformationRepository = customerAdditionalInformationRepository;
            this.nodaTimeZoneLocationBusiness = nodaTimeZoneLocationBusiness;
        }

        public async Task<Response<CustomerAdditionalInformationModel>> GetCustomerAdditionalInformation(string customerId)
        {
            return await this.customerAdditionalInformationRepository.GetCustomerAdditionalInformation(customerId);
        }

        public async Task<Response<bool>> UpdateCustomerAdditionalInformation(string customerId, CustomerAdditionalInformationModel customerAdditionalInformationModel)
        {
            var nodaTimeZoneLocationConfiguration = this.nodaTimeZoneLocationBusiness.GetNodaTimeZoneLocationInformation();
            if (nodaTimeZoneLocationConfiguration.ZoneWithCountryId.ContainsKey(customerAdditionalInformationModel.TimeZone))
            {
                return await this.customerAdditionalInformationRepository.UpdateCustomerAdditionalInformation(customerId, customerAdditionalInformationModel);
            }

            return Response<bool>.ValidationError(new List<string>() { AppBusinessMessagesConstants.InValidTimeZone });
        }
    }
}
