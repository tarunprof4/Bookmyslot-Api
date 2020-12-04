using System;
using System.Collections.Generic;
using System.Text;

namespace Bookmyslot.Api.Customers.Repositories.Enitites
{
    public class CustomerEntity
    {
        public string Prefix { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }
    }
}
