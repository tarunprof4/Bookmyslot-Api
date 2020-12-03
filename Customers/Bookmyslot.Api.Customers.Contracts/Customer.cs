using System;

namespace Bookmyslot.Api.Customers.Contracts
{
    public class Customer
    {
        public string Prefix { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }
    }
}
