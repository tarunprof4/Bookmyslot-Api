using Bookmyslot.Api.Common.Contracts.Constants;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bookmyslot.Api.Customers.ViewModels
{
    public class AdditionalProfileSettingsViewModel
    {
        [Required(ErrorMessage = AppBusinessMessagesConstants.BioHeadLineRequired)]
        [MaxLength(AppBusinessConstants.BioHeadLineMaxLength, ErrorMessage = AppBusinessMessagesConstants.BioHeadLineMaxLength)]
        [DefaultValue("BioHeadLine")]
        public string BioHeadLine { get; set; }

        public AdditionalProfileSettingsViewModel(string bioHeadLine)
        {
            this.BioHeadLine = bioHeadLine;
        }

    }
}
