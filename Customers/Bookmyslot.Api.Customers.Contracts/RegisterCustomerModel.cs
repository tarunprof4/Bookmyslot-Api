using Bookmyslot.Api.Common.Contracts.Constants;
using System;
using System.ComponentModel.DataAnnotations;

namespace Bookmyslot.Api.Customers.Contracts
{
    public class RegisterCustomerModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = AppBusinessMessagesConstants.FirstNameRequired)]
        [MaxLength(AppBusinessConstants.NameMaxLength, ErrorMessage = AppBusinessMessagesConstants.FirstNameMaxLength)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = AppBusinessMessagesConstants.LastNameRequired)]
        [MaxLength(AppBusinessConstants.NameMaxLength, ErrorMessage = AppBusinessMessagesConstants.LastNameMaxLength)]
        public string LastName { get; set; }

        [MaxLength(AppBusinessConstants.GenderMaxLength, ErrorMessage = AppBusinessMessagesConstants.GenderMaxLength)]
        public string Gender { get; set; }

        [MaxLength(AppBusinessConstants.UserNameMaxLength, ErrorMessage = AppBusinessMessagesConstants.UserNameMaxLength)]
        public string UserName { get; set; }

        [Required(ErrorMessage = AppBusinessMessagesConstants.EmailRequired)]
        [MaxLength(AppBusinessConstants.EmailMaxLength, ErrorMessage = AppBusinessMessagesConstants.EmailMaxLength)]
        public string Email { get; set; }

        [MaxLength(AppBusinessConstants.PhoneMaxLength, ErrorMessage = AppBusinessMessagesConstants.PhoneMaxLength)]
        public string PhoneNumber { get; set; }

        [MaxLength(AppBusinessConstants.BioHeadLineMaxLength, ErrorMessage = AppBusinessMessagesConstants.BioHeadLineMaxLength)]
        public string BioHeadLine { get; set; }

        [MaxLength(AppBusinessConstants.SocialProviderMaxLength, ErrorMessage = AppBusinessMessagesConstants.BioHeadLineMaxLength)]
        public string Provider { get; set; }

        public string PhotoUrl { get; set; }

        public DateTime CreatedDateUtc { get; set; }
    }
}
