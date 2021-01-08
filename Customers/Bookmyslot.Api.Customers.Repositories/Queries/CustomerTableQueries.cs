namespace Bookmyslot.Api.SlotScheduler.Repositories.Queries
{

    public class CustomerTableQueries
    {

        public const string GetCustomersByCustomerIdsQuery = @"select * from Customer where UniqueId IN @CustomerIds";

        public const string GetCustomerByEmailsQuery = @"select * from Customer where Email = @Email";

    }
}
