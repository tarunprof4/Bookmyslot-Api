using Bookmyslot.Api.Common.Database.Constants;

namespace Bookmyslot.Api.SlotScheduler.Repositories.Queries
{

    public class SlotTableQueries
    {

        public const string CreateSlotQuery = @"INSERT INTO" + " " + DatabaseConstants.SlotTable + " " +
            @"(Id, Title, CreatedBy, TimeZone, SlotDate, SlotDateUtc, SlotStartTime, SlotEndTime, CreatedDateUtc, IsDeleted)
 VALUES(@Id, @Title, @CreatedBy, @TimeZone, @SlotDate, @SlotDateUtc, @SlotStartTime, @SlotEndTime, @CreatedDateUtc, @IsDeleted); ";


        public const string InsertOrUpdateCustomerLastBookedSlotQuery = @"INSERT INTO" + " " + DatabaseConstants.CustomerLastBookedSlotTable + " " +
        @"(CreatedBy, Title, Country, TimeZone, SlotDate, SlotDateUtc, SlotStartTime , SlotEndTime, ModifiedDateUtc) 
	   VALUES(@CreatedBy, @Title, @Country, @TimeZone,@SlotDate,@SlotDateUtc,@SlotStartTime, @SlotEndTime, @ModifiedDateUtc) ON DUPLICATE KEY UPDATE
 Title=@Title, Country = @Country, TimeZone = @TimeZone, SlotDate = @SlotDate, SlotDateUtc = @SlotDateUtc, SlotStartTime = @SlotStartTime, SlotEndTime = @SlotEndTime, ModifiedDateUtc = @ModifiedDateUtc ";

        public const string GetSlotQuery = @"SELECT * FROM" + " " + DatabaseConstants.SlotTable + " " + @"where CreatedBy = @CreatedBy";

        public const string GetCustomerLastBookedSlotQuery = @"SELECT * FROM" + " " + DatabaseConstants.CustomerLastBookedSlotTable + " " + @"where Id = @Id";



        public const string UpdateSlotQuery = @"UPDATE" + " " + DatabaseConstants.SlotTable + " " + @"SET  
 BookedBy = @BookedBy, ModifiedDateUtc= @ModifiedDateUtc WHERE Id=@Id";

        public const string DeleteSlotQuery = @"UPDATE" + " " + DatabaseConstants.SlotTable + " " + @"SET  
 IsDeleted = @IsDeleted, ModifiedDateUtc= @ModifiedDateUtc WHERE Id=@Id";


        public const string CreateCancelledSlotQuery = @"INSERT INTO" + " " + DatabaseConstants.CancelledSlotTable + " " + @"(Id, Title, CreatedBy, CancelledBy, BookedBy, TimeZone, SlotDate, SlotDateUtc, SlotStartTime, SlotEndTime, CreatedDateUtc)
 VALUES(@Id, @Title, @CreatedBy, @CancelledBy, @BookedBy, @TimeZone, @SlotDate, @SlotDateUtc, @SlotStartTime, @SlotEndTime, @CreatedDateUtc); ";



        public const string GetAllSlotsQuery = @"SELECT * FROM" + " " + DatabaseConstants.SlotTable + " " + "order by SlotDateUtc OFFSET @PageNumber ROWS FETCH Next @PageSize ROWS ONLY";



        public const string GetDistinctCustomersNearestSlotFromTodayQuery = @"select CreatedBy from(
SELECT id, title, CreatedBy, SlotStartTime, SlotEndTime, IsDeleted, ModifiedDateUtc, TimeZone, SlotDateUtc,
ROW_NUMBER() OVER(PARTITION BY CreatedBy ORDER BY SlotDateUtc ASC) AS RowNumber FROM" + " " + DatabaseConstants.SlotTable + " " + @" where IsDeleted = @IsDeleted  and SlotDateUtc >  UTC_TIMESTAMP()
and (BookedBy  is Null or BookedBy = '')
)  as resultSet where resultSet.RowNumber = 1 order by resultSet.Id ASC LIMIT @PageSize OFFSET @PageNumber";






        public const string GetCustomerAvailableSlotsFromTodayQuery = @"SELECT * FROM" + " " + DatabaseConstants.SlotTable + " " + @"where IsDeleted=@IsDeleted and CreatedBy= @CreatedBy and SlotDateUtc > UTC_TIMESTAMP() and (BookedBy  is Null or BookedBy = '') order by SlotDateUtc, SlotStartTime
    LIMIT @PageSize OFFSET @PageNumber";


        public const string GetCustomerSharedByYetToBeBookedSlotsQuery = @"SELECT * FROM" + " " + DatabaseConstants.SlotTable + " " + "where IsDeleted=@IsDeleted and (BookedBy  is Null or BookedBy = '') and CreatedBy=@CreatedBy and SlotDateUtc > UTC_TIMESTAMP() order by SlotDateUtc, SlotStartTime";


        public const string GetCustomerSharedByBookedSlotsQuery = @"SELECT * FROM" + " " + DatabaseConstants.SlotTable + " " + "where IsDeleted=@IsDeleted and (BookedBy  is Not Null and BookedBy != '') and CreatedBy=@CreatedBy and SlotDateUtc > UTC_TIMESTAMP() order by SlotDateUtc, SlotStartTime";


        public const string GetCustomerSharedByCompletedSlotsQuery = @"SELECT * FROM Slot" + " " + DatabaseConstants.SlotTable + " " + "IsDeleted=@IsDeleted and (BookedBy  is Not Null and BookedBy != '') and CreatedBy=@CreatedBy and SlotDateUtc < UTC_TIMESTAMP() order by SlotDateUtc Desc, SlotStartTime Desc";

        public const string GetCustomerSharedByCancelledSlotsQuery = @"SELECT * FROM" + " " + DatabaseConstants.CancelledSlotTable + " " + "where  CancelledBy=@CancelledBy  order by SlotDateUtc Desc, SlotStartTime Desc";


        public const string GetCustomerBookedByBookedSlotsQuery = @"SELECT * FROM" + " " + DatabaseConstants.SlotTable + " " + "where IsDeleted=@IsDeleted and BookedBy=@BookedBy and SlotDateUtc > UTC_TIMESTAMP() order by SlotDateUtc, SlotStartTime";


        public const string GetCustomerBookedByCompletedSlotsQuery = @"SELECT * FROM" + " " + DatabaseConstants.SlotTable + " " + "where IsDeleted=@IsDeleted and BookedBy=@BookedBy and SlotDateUtc < UTC_TIMESTAMP() order by SlotDateUtc Desc, SlotStartTime Desc";


        public const string GetCustomerBookedByCancelledSlotsQuery = @"SELECT * FROM" + " " + DatabaseConstants.CancelledSlotTable + " " + "where  CancelledBy=@CancelledBy  or BookedBy=@BookedBy order by SlotDateUtc Desc, SlotStartTime Desc";
    }
}
