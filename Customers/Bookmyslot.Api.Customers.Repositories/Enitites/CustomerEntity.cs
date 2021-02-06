using Dapper;
using System;

namespace Bookmyslot.Api.Customers.Repositories.Enitites
{
    [Table("Customer")]
    public class CustomerEntity
    {
        [Key]
        public string UniqueId { get; set; }


        public string FirstName { get; set; }

        public string LastName { get; set; }


        public string Email { get; set; }

        public string BioHeadLine { get; set; }

        public DateTime CreatedDateUtc { get; set; }
        public DateTime? ModifiedDateUtc { get; set; }
    }
}
