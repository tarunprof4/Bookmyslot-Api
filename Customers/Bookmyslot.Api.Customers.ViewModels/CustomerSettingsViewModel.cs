using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bookmyslot.Api.Customers.ViewModels
{
    public class CustomerSettingsViewModel
    {
        [DefaultValue("Asia/Kolkata")]
        [Required]
        public string TimeZone { get; set; }
    }
}
