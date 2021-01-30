using Bookmyslot.Api.Common.Contracts.Constants;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Bookmyslot.Api.Customers.Contracts
{
    public class CustomerModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = AppBusinessMessages.FirstNameRequired)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = AppBusinessMessages.LastNameRequired)]
        public string LastName { get; set; }

        [Required]
        public string Gender { get; set; }

        public string Email { get; set; }

        public string BioHeadLine { get; set; }

        public DateTime CreatedDateUtc { get; set; }
    }
}
