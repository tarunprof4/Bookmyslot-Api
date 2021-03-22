﻿namespace Bookmyslot.Api.Customers.Contracts
{
    public class RegisterCustomerModel
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Provider { get; set; }

        public bool IsVerified { get; set; }
    }
}
