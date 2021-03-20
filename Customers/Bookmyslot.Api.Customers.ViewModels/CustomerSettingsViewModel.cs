using Bookmyslot.Api.Common.Contracts.Constants;
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
    }
}
