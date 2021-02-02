using Bookmyslot.Api.Common.Contracts.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Bookmyslot.Api.Customers.Contracts
{
    public class RegisterSocialUser 
    {
        public string Id { get; set; }

        [Required(ErrorMessage = AppBusinessMessages.FirstNameRequired)]
        [MaxLength(AppBusinessMessages.NameMaxLength, ErrorMessage = AppBusinessMessages.FirstNameMaxLength)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = AppBusinessMessages.LastNameRequired)]
        [MaxLength(AppBusinessMessages.NameMaxLength, ErrorMessage = AppBusinessMessages.LastNameMaxLength)]
        public string LastName { get; set; }

        public string Email { get; set; }
    }
}
