using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Customers.Contracts;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bookmyslot.Api.Customers.ViewModels
{
    public class CustomerSettingsViewModel
    {
        [DefaultValue(CountryConstants.India)]
        [Required]
        public string Country { get; set; }

        [DefaultValue(TimeZoneConstants.IndianTimezone)]
        [Required]
        public string TimeZone { get; set; }

        public static CustomerSettingsViewModel CreateCustomerSettingsViewModel(CustomerSettingsModel customerSettingsModel)
        {
            var customerSettingsViewModel = new CustomerSettingsViewModel
            {
                TimeZone = customerSettingsModel.TimeZone,
            };

            return customerSettingsViewModel;
        }
    }
}
