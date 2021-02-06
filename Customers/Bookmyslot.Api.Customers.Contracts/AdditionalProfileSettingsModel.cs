using Bookmyslot.Api.Common.Contracts.Constants;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bookmyslot.Api.Customers.Contracts
{
    public class AdditionalProfileSettingsModel
    {
        [Required(ErrorMessage = AppBusinessMessagesConstants.BioHeadLineRequired)]
        [MaxLength(AppBusinessConstants.BioHeadLineMaxLength, ErrorMessage = AppBusinessMessagesConstants.BioHeadLineMaxLength)]
        [DefaultValue("BioHeadLine")]
        public string BioHeadLine { get; set; }
    }
}
