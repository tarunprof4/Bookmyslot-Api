using Bookmyslot.Api.Common.Database.Constants;

namespace Bookmyslot.Api.Customers.Repositories.Queries
{

    public class CustomerTableQueries
    {
        public const string UpdateProfileSettingQuery = @"UPDATE"+ " "+ TableNameConstants.Register + " " + @"SET 
 FirstName = @FirstName,  LastName = @LastName, Gender=@Gender, ModifiedDateUtc =@ModifiedDateUtc
 WHERE UniqueId=@customerId";

        public const string GetCustomersByCustomerIdsQuery = @"select UniqueId, Email, FirstName, LastName, BioHeadLine from" + " " + TableNameConstants.Register + " " + "where UniqueId IN @CustomerIds";

        public const string GetCustomerByEmailQuery = @"select UniqueId, Email, FirstName, LastName, BioHeadLine from" + " " + TableNameConstants.Register + " " + "where Email = @Email";

        public const string GetCustomerIdByEmailQuery = @"select UniqueId from" + " " + TableNameConstants.Register + " " + "where Email = @Email";

        public const string GetCustomerByIdQuery = @"select UniqueId, Email, FirstName, LastName, BioHeadLine from" + " " + TableNameConstants.Register + " " + "where UniqueId = @CustomerId";

        public const string GetProfileSettingsByCustomerIdQuery = @"select Email, FirstName, LastName, Gender from" + " " + TableNameConstants.Register + " " + "where UniqueId=@customerId";

        public const string GetCustomerAdditionInformationQuery = @"select timeZone from" + " " + TableNameConstants.CustomerAdditionalInformation + " " + "where CustomerId=@customerId";

        public const string InsertOrUpdateCustomerAdditionInformationQuery = @"UPDATE" + " " + TableNameConstants.CustomerAdditionalInformation + " " + @"SET 
  TimeZone = @timeZone, ModifiedDateUtc =@ModifiedDateUtc
  WHERE CustomerId=@customerId";


    }
}
