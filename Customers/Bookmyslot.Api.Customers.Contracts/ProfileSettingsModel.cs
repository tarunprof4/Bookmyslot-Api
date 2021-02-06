using Bookmyslot.Api.Common.Contracts.Constants;
using System.ComponentModel.DataAnnotations;

namespace Bookmyslot.Api.Customers.Contracts
{
    public class ProfileSettingsModel
    {
        public string Email { get; set; }

        [Required(ErrorMessage = AppBusinessMessagesConstants.FirstNameRequired)]
        [MaxLength(AppBusinessConstants.NameMaxLength, ErrorMessage = AppBusinessMessagesConstants.FirstNameMaxLength)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = AppBusinessMessagesConstants.LastNameRequired)]
        [MaxLength(AppBusinessConstants.NameMaxLength, ErrorMessage = AppBusinessMessagesConstants.LastNameMaxLength)]
        public string LastName { get; set; }

        [Required(ErrorMessage = AppBusinessMessagesConstants.GenderRequired)]
        [MaxLength(AppBusinessConstants.GenderMaxLength, ErrorMessage = AppBusinessMessagesConstants.GenderMaxLength)]
        public string Gender { get; set; }

        [Required(ErrorMessage = AppBusinessMessagesConstants.BioHeadLineRequired)]
        [MaxLength(AppBusinessConstants.BioHeadLineMaxLength, ErrorMessage = AppBusinessMessagesConstants.BioHeadLineMaxLength)]
        public string BioHeadLine { get; set; }
    }
}
