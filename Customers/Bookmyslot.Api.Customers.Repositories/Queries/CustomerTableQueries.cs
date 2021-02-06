namespace Bookmyslot.Api.SlotScheduler.Repositories.Queries
{

    public class CustomerTableQueries
    {
        public const string UpdateProfileSettingQuery = @"   UPDATE Customer
FirstName = @FirstName,  LastName = @LastName, Gender=@Gender, BioHeadLine=@BioHeadLine, ModifiedDateUtc =@ModifiedDateUtc
WHERE UniqueId=@customerId";

        public const string GetCustomersByCustomerIdsQuery = @"select * from Customer where UniqueId IN @CustomerIds";

        public const string GetCustomerByEmailsQuery = @"select * from Customer where Email = @Email";

    }
}
