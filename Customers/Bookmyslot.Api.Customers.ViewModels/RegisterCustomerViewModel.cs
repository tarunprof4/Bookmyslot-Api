using Bookmyslot.Api.Common.Contracts.Constants;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bookmyslot.Api.Customers.ViewModels
{
    public class RegisterCustomerViewModel
    {
        [Required(ErrorMessage = AppBusinessMessagesConstants.FirstNameRequired)]
        [MaxLength(AppBusinessConstants.NameMaxLength, ErrorMessage = AppBusinessMessagesConstants.FirstNameMaxLength)]
        [DefaultValue("FirstNamee")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = AppBusinessMessagesConstants.LastNameRequired)]
        [MaxLength(AppBusinessConstants.NameMaxLength, ErrorMessage = AppBusinessMessagesConstants.LastNameMaxLength)]
        [DefaultValue("LastNamee")]
        public string LastName { get; set; }

        [MaxLength(AppBusinessConstants.UserNameMaxLength, ErrorMessage = AppBusinessMessagesConstants.UserNameMaxLength)]
        public string UserName { get; set; }

        [Required(ErrorMessage = AppBusinessMessagesConstants.EmailRequired)]
        [MaxLength(AppBusinessConstants.EmailMaxLength, ErrorMessage = AppBusinessMessagesConstants.EmailMaxLength)]
        [DefaultValue("a@gmail.com")]
        public string Email { get; set; }

        [MaxLength(AppBusinessConstants.ProviderMaxLength, ErrorMessage = AppBusinessMessagesConstants.ProviderMaxLength)]
        public string Provider { get; set; }

        public string PhotoUrl { get; set; }
    }
}
