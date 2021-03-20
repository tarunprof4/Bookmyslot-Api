using Bookmyslot.Api.Common.Database.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookmyslot.Api.Search.Repositories.Enitites
{
    [Table(DatabaseConstants.RegisterCustomerTable)]
    public class SearchCustomerEntity
    {
        [Key]

        public string Id { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhotoUrl { get; set; }

    }
}
