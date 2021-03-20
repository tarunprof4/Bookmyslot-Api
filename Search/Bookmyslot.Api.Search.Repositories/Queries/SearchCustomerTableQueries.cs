using Bookmyslot.Api.Common.Database.Constants;

namespace Bookmyslot.Api.SlotScheduler.Repositories.Queries
{

    public class SearchCustomerTableQueries
    {
        public const string InsertOrUpdatePreProcessedSearchedCustomerQuery = @"INSERT INTO" + " " + DatabaseConstants.SearchTable + " " +
            @"(SearchKey,Value,ModifiedDateUtc) VALUES(@SearchKey, @Value,@ModifiedDateUtc) ON DUPLICATE KEY UPDATE
 SearchKey=@SearchKey, Value=@Value,  ModifiedDateUtc = @ModifiedDateUtc";


        public const string GetPreProcessedSearchedCustomerQuery = @"select * from" + " " + DatabaseConstants.SearchTable;


        public const string SearchCustomerQuery = @"select UniqueId, UserName, FirstName, LastName, PhotoUrl from" + " " + DatabaseConstants.RegisterCustomerTable;


    }
}
