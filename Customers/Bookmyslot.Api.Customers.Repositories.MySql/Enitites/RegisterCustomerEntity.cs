using System;

namespace Bookmyslot.Api.Customers.Repositories.Enitites
{
    public class RegisterCustomerEntity
    {
        public string Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Gender { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string BioHeadLine { get; set; }

        public string Provider { get; set; }

        public string ProfilePictureUrl { get; set; }

        public DateTime CreatedDateUtc { get; set; }

        public bool IsVerified { get; set; }
    }
}
