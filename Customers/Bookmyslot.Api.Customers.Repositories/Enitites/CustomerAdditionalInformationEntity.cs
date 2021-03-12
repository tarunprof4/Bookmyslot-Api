using Bookmyslot.Api.Common.Database.Constants;
using Dapper;
using System;

namespace Bookmyslot.Api.Customers.Repositories.Enitites
{
    [Table(TableNameConstants.CustomerAdditionalInformation)]
    public class CustomerAdditionalInformationEntity
    {
        public string CustomerId { get; set; }

        public string TimeZone { get; set; }

        public DateTime ModifiedDateUtc { get; set; }
    }
}
