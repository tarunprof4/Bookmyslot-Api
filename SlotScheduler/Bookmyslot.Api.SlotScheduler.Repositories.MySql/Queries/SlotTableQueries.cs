using Bookmyslot.Api.Common.Database.Constants;

namespace Bookmyslot.Api.SlotScheduler.Repositories.Queries
{

    public class SlotTableQueries
    {

        public const string CreateCancelledSlotQuery = @"INSERT INTO" + " " + TableNameConstants.CancelledSlot + " " +@"(Id, Title, CreatedBy, CancelledBy, BookedBy, TimeZone, SlotDate, SlotDateUtc, SlotStartTime, SlotEndTime, CreatedDateUtc)
 VALUES(@Id, @Title, @CreatedBy, @CancelledBy, @BookedBy, @TimeZone, @SlotDate, @SlotDateUtc, @SlotStartTime, @SlotEndTime, @CreatedDateUtc); ";



        public const string GetAllSlotsQuery = @"SELECT * FROM" + " " + TableNameConstants.Slot + " " + "order by SlotDateUtc OFFSET @PageNumber ROWS FETCH Next @PageSize ROWS ONLY";

   

        public const string GetDistinctCustomersNearestSlotFromTodayQuery = @"select CreatedBy from(
SELECT id, title, CreatedBy, SlotStartTime, SlotEndTime, IsDeleted, ModifiedDateUtc, TimeZone, SlotDateUtc,
ROW_NUMBER() OVER(PARTITION BY CreatedBy ORDER BY SlotDateUtc ASC) AS RowNumber FROM" + " " + TableNameConstants.Slot + " " + @" where IsDeleted = @IsDeleted  and SlotDateUtc >  UTC_TIMESTAMP()
and (BookedBy  is Null or BookedBy = '')
)  as resultSet where resultSet.RowNumber = 1 order by resultSet.Id ASC LIMIT @PageSize OFFSET @PageNumber";






        public const string GetCustomerAvailableSlotsFromTodayQuery = @"SELECT * FROM" + " " + TableNameConstants.Slot + " " + @"where IsDeleted=@IsDeleted and CreatedBy= @CreatedBy and SlotDateUtc > UTC_TIMESTAMP() and (BookedBy  is Null or BookedBy = '') order by SlotDateUtc, SlotStartTime
    LIMIT @PageSize OFFSET @PageNumber";


        public const string GetCustomerSharedByYetToBeBookedSlotsQuery = @"SELECT * FROM" + " " + TableNameConstants.Slot + " " + "where IsDeleted=@IsDeleted and (BookedBy  is Null or BookedBy = '') and CreatedBy=@CreatedBy and SlotDateUtc > UTC_TIMESTAMP() order by SlotDateUtc, SlotStartTime";


        public const string GetCustomerSharedByBookedSlotsQuery = @"SELECT * FROM" + " " + TableNameConstants.Slot + " " + "where IsDeleted=@IsDeleted and (BookedBy  is Not Null and BookedBy != '') and CreatedBy=@CreatedBy and SlotDateUtc > UTC_TIMESTAMP() order by SlotDateUtc, SlotStartTime";


        public const string GetCustomerSharedByCompletedSlotsQuery = @"SELECT * FROM Slot" + " " + TableNameConstants.Slot + " " + "IsDeleted=@IsDeleted and (BookedBy  is Not Null and BookedBy != '') and CreatedBy=@CreatedBy and SlotDateUtc < UTC_TIMESTAMP() order by SlotDateUtc Desc, SlotStartTime Desc";

        public const string GetCustomerSharedByCancelledSlotsQuery = @"SELECT * FROM" + " " + TableNameConstants.CancelledSlot + " " + "where  CancelledBy=@CancelledBy  order by SlotDateUtc Desc, SlotStartTime Desc";


        public const string GetCustomerBookedByBookedSlotsQuery = @"SELECT * FROM" + " " + TableNameConstants.Slot + " " + "where IsDeleted=@IsDeleted and BookedBy=@BookedBy and SlotDateUtc > UTC_TIMESTAMP() order by SlotDateUtc, SlotStartTime";


        public const string GetCustomerBookedByCompletedSlotsQuery = @"SELECT * FROM" + " " + TableNameConstants.Slot + " " + "where IsDeleted=@IsDeleted and BookedBy=@BookedBy and SlotDateUtc < UTC_TIMESTAMP() order by SlotDateUtc Desc, SlotStartTime Desc";


        public const string GetCustomerBookedByCancelledSlotsQuery = @"SELECT * FROM" + " " + TableNameConstants.CancelledSlot + " " + "where  CancelledBy=@CancelledBy  or BookedBy=@BookedBy order by SlotDateUtc Desc, SlotStartTime Desc";
    }
}
