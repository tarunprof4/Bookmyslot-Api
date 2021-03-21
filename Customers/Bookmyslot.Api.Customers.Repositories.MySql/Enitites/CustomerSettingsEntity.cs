using System;

namespace Bookmyslot.Api.Customers.Repositories.Enitites
{
    public class CustomerSettingsEntity
    {
        public string CustomerId { get; set; }

        public string Country { get; set; }
        public string TimeZone { get; set; }

        public DateTime ModifiedDateUtc { get; set; }
    }
}
