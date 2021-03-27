﻿using Bookmyslot.Api.Common.Database.Constants;

namespace Bookmyslot.Api.SlotScheduler.Repositories.Queries
{

    public class SearchQueries
    {
        public const string InsertOrUpdatePreProcessedSearchedCustomerQuery = @"INSERT INTO" + " " + DatabaseConstants.SearchTable + " " +
            @"(SearchKey,Value,ModifiedDateUtc) VALUES(@SearchKey, @Value,@ModifiedDateUtc) ON DUPLICATE KEY UPDATE
 SearchKey=@SearchKey, Value=@Value,  ModifiedDateUtc = @ModifiedDateUtc";


        public const string GetPreProcessedSearchedCustomerQuery = @"select * from" + " " + DatabaseConstants.SearchTable;


        public const string SearchCustomerByBioHeadLineQuery = @"SELECT  firstname, lastname, PhotoUrl, UserName, MATCH (BioHeadLine) AGAINST
    (@bioHeadLine IN NATURAL LANGUAGE MODE) AS score
    FROM" + " " + DatabaseConstants.RegisterCustomerTable + " " + @"WHERE MATCH (BioHeadLine) AGAINST
    (@bioHeadLine IN NATURAL LANGUAGE MODE) Limit 10;";


        public const string SearchCustomerByNameQuery = @"SELECT * from (SELECT  firstname, lastname, PhotoUrl, UserName, MATCH (FirstName, LastName) 
AGAINST (@name IN BOOLEAN MODE) AS score FROM" + " " + DatabaseConstants.RegisterCustomerTable + " " +
            @"ORDER BY score DESC Limit 10)  as derived where score>0;";



        public const string SearchCustomerByUserNameQuery = @"SELECT  firstname, lastname, PhotoUrl,UserName  MATCH (BioHeadLine) AGAINST
    (@bioHeadLine IN NATURAL LANGUAGE MODE) AS score
    FROM" + " " + DatabaseConstants.RegisterCustomerTable + " " + @"WHERE MATCH (BioHeadLine) AGAINST
    (@bioHeadLine IN NATURAL LANGUAGE MODE) Limit 10;";


    }
}
