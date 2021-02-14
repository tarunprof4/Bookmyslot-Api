﻿using Bookmyslot.Api.Common.Contracts.Constants;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bookmyslot.Api.Customers.Contracts
{
    public class RegisterCustomerModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = AppBusinessMessagesConstants.FirstNameRequired)]
        [MaxLength(AppBusinessConstants.NameMaxLength, ErrorMessage = AppBusinessMessagesConstants.FirstNameMaxLength)]
        [DefaultValue("FirstNamee")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = AppBusinessMessagesConstants.LastNameRequired)]
        [MaxLength(AppBusinessConstants.NameMaxLength, ErrorMessage = AppBusinessMessagesConstants.LastNameMaxLength)]
        [DefaultValue("LastNamee")]
        public string LastName { get; set; }

        [MaxLength(AppBusinessConstants.GenderMaxLength, ErrorMessage = AppBusinessMessagesConstants.GenderMaxLength)]
        public string Gender { get; set; }

        [MaxLength(AppBusinessConstants.UserNameMaxLength, ErrorMessage = AppBusinessMessagesConstants.UserNameMaxLength)]
        public string UserName { get; set; }

        [Required(ErrorMessage = AppBusinessMessagesConstants.EmailRequired)]
        [MaxLength(AppBusinessConstants.EmailMaxLength, ErrorMessage = AppBusinessMessagesConstants.EmailMaxLength)]
        [DefaultValue("a@gmail.com")]
        public string Email { get; set; }

        [MaxLength(AppBusinessConstants.PhoneMaxLength, ErrorMessage = AppBusinessMessagesConstants.PhoneMaxLength)]
        public string PhoneNumber { get; set; }

        [MaxLength(AppBusinessConstants.BioHeadLineMaxLength, ErrorMessage = AppBusinessMessagesConstants.BioHeadLineMaxLength)]
        public string BioHeadLine { get; set; }

        [MaxLength(AppBusinessConstants.ProviderMaxLength, ErrorMessage = AppBusinessMessagesConstants.ProviderMaxLength)]
        public string Provider { get; set; }

        public string PhotoUrl { get; set; }

        public DateTime CreatedDateUtc { get; set; }
    }
}
