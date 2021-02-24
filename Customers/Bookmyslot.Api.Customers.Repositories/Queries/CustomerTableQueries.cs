namespace Bookmyslot.Api.Customers.Repositories.Queries
{

    public class CustomerTableQueries
    {
        public const string UpdateProfileSettingQuery = @"UPDATE Customer SET 
 FirstName = @FirstName,  LastName = @LastName, Gender=@Gender, ModifiedDateUtc =@ModifiedDateUtc
 WHERE UniqueId=@customerId";

        public const string GetCustomersByCustomerIdsQuery = @"select UniqueId, Email, FirstName, LastName, BioHeadLine from Customer where UniqueId IN @CustomerIds";

        public const string GetCustomerByEmailQuery = @"select UniqueId, Email, FirstName, LastName, BioHeadLine from Customer where Email = @Email";

        public const string GetCustomerIdByEmailQuery = @"select UniqueId from Customer where Email = @Email";

        public const string GetCustomerByIdQuery = @"select UniqueId, Email, FirstName, LastName, BioHeadLine from Customer where UniqueId = @CustomerId";

        public const string GetProfileSettingsByCustomerIdQuery = @"select Email, FirstName, LastName, Gender from Customer where UniqueId=@customerId";

    }
}
