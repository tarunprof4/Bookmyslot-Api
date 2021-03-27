using Bookmyslot.Api.Common.Database.Constants;

namespace Bookmyslot.Api.SlotScheduler.Repositories.Queries
{

    public class SlotTableQueries
    {

        public const string CreateSlotQuery = @"INSERT INTO" + " " + DatabaseConstants.SlotTable + " " +
            @"(Id, Title, CreatedBy, TimeZone, SlotDate, SlotStartDateTimeUtc,SlotEndDateTimeUtc, SlotStartTime, SlotEndTime, CreatedDateUtc, IsDeleted, Country)
 VALUES(@Id, @Title, @CreatedBy, @TimeZone, @SlotDate, @SlotStartDateTimeUtc, @SlotEndDateTimeUtc, @SlotStartTime, @SlotEndTime, @CreatedDateUtc, @IsDeleted, @Country); ";
 

        public const string GetSlotQuery = @"SELECT * FROM" + " " + DatabaseConstants.SlotTable + " " + @"where IsDeleted=@IsDeleted and Id=@Id";


        public const string UpdateSlotQuery = @"UPDATE" + " " + DatabaseConstants.SlotTable + " " + @"SET  
 BookedBy = @BookedBy, ModifiedDateUtc= @ModifiedDateUtc, SlotMeetingLink= @SlotMeetingLink WHERE Id=@Id";

        public const string DeleteSlotQuery = @"UPDATE" + " " + DatabaseConstants.SlotTable + " " + @"SET  
 IsDeleted = @IsDeleted, ModifiedDateUtc= @ModifiedDateUtc WHERE Id=@Id";


        public const string GetDistinctCustomersNearestSlotFromTodayQuery = @"select CreatedBy from(
SELECT id, title, CreatedBy, SlotStartTime, SlotEndTime, IsDeleted, ModifiedDateUtc, TimeZone, SlotStartDateTimeUtc,
ROW_NUMBER() OVER(PARTITION BY CreatedBy ORDER BY SlotStartDateTimeUtc ASC) AS RowNumber FROM" + " " + DatabaseConstants.SlotTable + " " + @" where IsDeleted = @IsDeleted  and SlotStartDateTimeUtc >  UTC_TIMESTAMP()
and (BookedBy  is Null or BookedBy = '')
)  as resultSet where resultSet.RowNumber = 1 order by resultSet.Id ASC LIMIT @PageSize OFFSET @PageNumber";


        public const string GetCustomerAvailableSlotsFromTodayQuery = @"SELECT * FROM" + " " + DatabaseConstants.SlotTable + " " + @"where IsDeleted=@IsDeleted and CreatedBy= @CreatedBy and SlotStartDateTimeUtc > UTC_TIMESTAMP() and (BookedBy  is Null or BookedBy = '') order by SlotStartDateTimeUtc, SlotStartTime
    LIMIT @PageSize OFFSET @PageNumber";


        public const string GetCustomerSharedByYetToBeBookedSlotsQuery = @"SELECT * FROM" + " " + DatabaseConstants.SlotTable + " " + "where IsDeleted=@IsDeleted and (BookedBy  is Null or BookedBy = '') and CreatedBy=@CreatedBy and SlotStartDateTimeUtc > UTC_TIMESTAMP() order by SlotStartDateTimeUtc, SlotStartTime";


        public const string GetCustomerSharedByBookedSlotsQuery = @"SELECT * FROM" + " " + DatabaseConstants.SlotTable + " " + "where IsDeleted=@IsDeleted and (BookedBy  is Not Null and BookedBy != '') and CreatedBy=@CreatedBy and SlotEndDateTimeUtc > UTC_TIMESTAMP() order by SlotStartDateTimeUtc, SlotStartTime";


        public const string GetCustomerSharedByCompletedSlotsQuery = @"SELECT * FROM" + " " + DatabaseConstants.SlotTable + " " + "where IsDeleted=@IsDeleted and (BookedBy  is Not Null and BookedBy != '') and CreatedBy=@CreatedBy and SlotEndDateTimeUtc < UTC_TIMESTAMP() order by SlotStartDateTimeUtc Desc, SlotStartTime Desc";



        public const string GetCustomerBookedByBookedSlotsQuery = @"SELECT * FROM" + " " + DatabaseConstants.SlotTable + " " + "where IsDeleted=@IsDeleted and BookedBy=@BookedBy and SlotEndDateTimeUtc > UTC_TIMESTAMP() order by SlotStartDateTimeUtc, SlotStartTime";


        public const string GetCustomerBookedByCompletedSlotsQuery = @"SELECT * FROM" + " " + DatabaseConstants.SlotTable + " " + "where IsDeleted=@IsDeleted and BookedBy=@BookedBy and SlotEndDateTimeUtc < UTC_TIMESTAMP() order by SlotStartDateTimeUtc Desc, SlotStartTime Desc";
    }
}
