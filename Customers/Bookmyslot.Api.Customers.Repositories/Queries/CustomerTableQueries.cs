namespace Bookmyslot.Api.SlotScheduler.Repositories.Queries
{

    public class CustomerTableQueries
    {

        public const string GetCustomersByEmailsQuery = @"select * from Customer where Email IN @Emails";

       

    }
}
