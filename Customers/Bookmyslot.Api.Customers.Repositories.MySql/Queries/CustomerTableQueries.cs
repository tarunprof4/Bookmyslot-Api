using Bookmyslot.Api.Common.Database.Constants;

namespace Bookmyslot.Api.Customers.Repositories.Queries
{

    public class CustomerTableQueries
    {

        public const string RegisterCustomerQuery = @"INSERT INTO" + " " + DatabaseConstants.RegisterCustomerTable + " " + @"(UniqueId, FirstName, LastName, UserName, Email, Provider, PhotoUrl, CreatedDateUtc)
VALUES(@UniqueId, @FirstName, @LastName, @UserName, @Email, @Provider, @PhotoUrl, @CreatedDateUtc); ";


        public const string UpdateProfileSettingQuery = @"UPDATE" + " " + DatabaseConstants.RegisterCustomerTable + " " + @"SET 
 FirstName = @FirstName,  LastName = @LastName, Gender=@Gender, ModifiedDateUtc =@ModifiedDateUtc
 WHERE UniqueId=@customerId";

        public const string GetCustomersByCustomerIdsQuery = @"select UniqueId, Email, FirstName, LastName, BioHeadLine from" + " " + DatabaseConstants.RegisterCustomerTable + " " + "where UniqueId IN @CustomerIds";

        public const string GetCustomerByEmailQuery = @"select UniqueId, Email, FirstName, LastName, BioHeadLine from" + " " + DatabaseConstants.RegisterCustomerTable + " " + "where Email = @Email";

        public const string GetCustomerIdByEmailQuery = @"select UniqueId from" + " " + DatabaseConstants.RegisterCustomerTable + " " + "where Email = @Email";

        public const string GetCustomerByIdQuery = @"select UniqueId, Email, FirstName, LastName, BioHeadLine from" + " " + DatabaseConstants.RegisterCustomerTable + " " + "where UniqueId = @CustomerId";

        public const string GetProfileSettingsByCustomerIdQuery = @"select Email, FirstName, LastName, Gender from" + " " + DatabaseConstants.RegisterCustomerTable + " " + "where UniqueId=@customerId";

        public const string GetCustomerSettingsQuery = @"select timeZone from" + " " + DatabaseConstants.CustomerSettingsTable + " " + "where CustomerId=@customerId";


        public const string InsertOrUpdateCustomerSettingsQuery = @"INSERT INTO" + " " + DatabaseConstants.CustomerSettingsTable + " " + @"(CustomerId, TimeZone,  ModifiedDateUtc) VALUES(@customerId, @timeZone, @ModifiedDateUtc) ON DUPLICATE KEY UPDATE
 TimeZone=@timeZone,  ModifiedDateUtc = @ModifiedDateUtc";

    }
}
