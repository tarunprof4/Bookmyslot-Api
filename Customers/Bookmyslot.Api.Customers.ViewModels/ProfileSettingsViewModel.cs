using Bookmyslot.Api.Common.Contracts.Constants;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bookmyslot.Api.Customers.ViewModels
{
    public class ProfileSettingsViewModel
    {
        [Required(ErrorMessage = AppBusinessMessagesConstants.FirstNameRequired)]
        [MaxLength(AppBusinessConstants.NameMaxLength, ErrorMessage = AppBusinessMessagesConstants.FirstNameMaxLength)]
        [DefaultValue("FirstNameee")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = AppBusinessMessagesConstants.LastNameRequired)]
        [MaxLength(AppBusinessConstants.NameMaxLength, ErrorMessage = AppBusinessMessagesConstants.LastNameMaxLength)]
        [DefaultValue("LastNameee")]
        public string LastName { get; set; }

        [Required(ErrorMessage = AppBusinessMessagesConstants.GenderRequired)]
        [MaxLength(AppBusinessConstants.GenderMaxLength, ErrorMessage = AppBusinessMessagesConstants.GenderMaxLength)]
        [DefaultValue("Gender")]
        public string Gender { get; set; }
    }
}
