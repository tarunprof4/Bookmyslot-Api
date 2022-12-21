using Bookmyslot.Api.Customers.Domain;
using Bookmyslot.SharedKernel.Constants;
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
                Country = customerSettingsModel.Country,
                TimeZone = customerSettingsModel.TimeZone,
            };

            return customerSettingsViewModel;
        }
    }
}
