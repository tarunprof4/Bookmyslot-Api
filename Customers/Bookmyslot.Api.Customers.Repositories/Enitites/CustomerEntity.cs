using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookmyslot.Api.Customers.Repositories.Enitites
{
    [Table("Customer")]
    public class CustomerEntity
    {
        [Key]
        public string Email { get; set; }

        public string GenderPrefix { get; set; }
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Gender { get; set; }

        //public DateTime ModifiedDate { get; set; }

    }
}
