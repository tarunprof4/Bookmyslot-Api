namespace Bookmyslot.Api.SlotScheduler.Repositories.Queries
{

    public class SlotTableQueries
    {

        public const string GetAllSlotsQuery = @"SELECT * FROM Slot order by SlotDate OFFSET @PageNumber ROWS FETCH Next @PageSize ROWS ONLY";

        public const string GetDistinctCustomersNearestSlotFromTodayQuery = @"select * from (
SELECT id, title, CreatedBy, SlotStartTime, SlotEndTime, IsDeleted, ModifiedDate, TimeZone, SlotDate,
       ROW_NUMBER() OVER(PARTITION BY CreatedBy ORDER BY slotDate ASC) AS RowNumber
FROM Slot where IsDeleted = @IsDeleted  and SlotDate > SYSUTCDATETIME() and (BookedBy  is Null or BookedBy = '')
)  as resultSet where resultSet.RowNumber = 1 order by resultSet.Id ASC OFFSET @PageNumber ROWS  FETCH Next @PageSize ROWS ONLY";


        public const string GetCustomerAvailableSlotsFromTodayQuery = @"SELECT * FROM Slot where IsDeleted=@IsDeleted and CreatedBy= @CreatedBy and SlotDate > SYSUTCDATETIME() and (BookedBy  is Null or BookedBy = '') order by SlotDate, SlotStartTime
   OFFSET @PageNumber ROWS FETCH Next @PageSize ROWS ONLY";


        public const string GetCustomerSharedByYetToBeBookedSlotsQuery = @"SELECT * FROM Slot where IsDeleted=@IsDeleted and (BookedBy  is Null or BookedBy = '') and CreatedBy=@CreatedBy and SlotDate > SYSUTCDATETIME() order by SlotDate, SlotStartTime";


        public const string GetCustomerSharedByBookedSlotsQuery = @"SELECT * FROM Slot where IsDeleted=@IsDeleted and (BookedBy  is Not Null and BookedBy != '') and CreatedBy=@CreatedBy and SlotDate > SYSUTCDATETIME() order by SlotDate, SlotStartTime";


        public const string GetCustomerSharedByCompletedSlotsQuery = @"SELECT * FROM Slot where IsDeleted=@IsDeleted and (BookedBy  is Not Null and BookedBy != '') and CreatedBy=@CreatedBy and SlotDate < SYSUTCDATETIME() order by SlotDate Desc, SlotStartTime Desc";

        public const string GetCustomerSharedByCancelledSlotsQuery = @"SELECT * FROM CancelledSlot where  CancelledBy=@CancelledBy  order by SlotDate Desc, SlotStartTime Desc";


        public const string GetCustomerBookedByBookedSlotsQuery = @"SELECT * FROM Slot where IsDeleted=@IsDeleted and BookedBy=@BookedBy and SlotDate > SYSUTCDATETIME() order by SlotDate, SlotStartTime";


        public const string GetCustomerBookedByCompletedSlotsQuery = @"SELECT * FROM Slot where IsDeleted=@IsDeleted and BookedBy=@BookedBy and SlotDate < SYSUTCDATETIME() order by SlotDate Desc, SlotStartTime Desc";


        public const string GetCustomerBookedByCancelledSlotsQuery = @"SELECT * FROM CancelledSlot where  CancelledBy=@CancelledBy  or BookedBy=@BookedBy order by SlotDate Desc, SlotStartTime Desc";
    }
}
