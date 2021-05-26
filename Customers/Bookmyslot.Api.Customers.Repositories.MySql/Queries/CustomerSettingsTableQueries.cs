using Bookmyslot.Api.Common.Contracts.Infrastructure.Database.Constants;

namespace Bookmyslot.Api.Customers.Repositories.Queries
{
    public class CustomerSettingsTableQueries
    {
        public const string GetCustomerSettingsQuery = @"select * from" + " " + DatabaseConstants.CustomerSettingsTable + " " + "where CustomerId=@customerId";


        public const string InsertOrUpdateCustomerSettingsQuery = @"INSERT INTO" + " " + DatabaseConstants.CustomerSettingsTable + " " + @"(CustomerId,Country, TimeZone,  ModifiedDateUtc) VALUES(@customerId, @Country, @timeZone, @ModifiedDateUtc) ON DUPLICATE KEY UPDATE
 Country=@Country, TimeZone=@timeZone,  ModifiedDateUtc = @ModifiedDateUtc";

    }
}
