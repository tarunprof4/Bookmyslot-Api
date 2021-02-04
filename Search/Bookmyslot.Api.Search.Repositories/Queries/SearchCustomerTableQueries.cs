namespace Bookmyslot.Api.SlotScheduler.Repositories.Queries
{

    public class SearchCustomerTableQueries
    {
        public const string SearchCustomerQuery = @"select UniqueId, FirstName, LastName from Customer";
    }
}
