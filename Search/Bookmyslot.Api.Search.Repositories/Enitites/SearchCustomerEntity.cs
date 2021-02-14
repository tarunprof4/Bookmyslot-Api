using Dapper;

namespace Bookmyslot.Api.Search.Repositories.Enitites
{
    [Table("Customer")]
    public class SearchCustomerEntity
    {
        [Key]
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

    }
}
