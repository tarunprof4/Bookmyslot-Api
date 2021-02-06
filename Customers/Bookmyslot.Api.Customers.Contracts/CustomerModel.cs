using System;

namespace Bookmyslot.Api.Customers.Contracts
{
    public class CustomerModel
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Gender { get; set; }

        public string Email { get; set; }

        public string BioHeadLine { get; set; }

        public DateTime CreatedDateUtc { get; set; }
    }
}
