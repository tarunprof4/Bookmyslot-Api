using Bookmyslot.Api.Common.Contracts.Infrastructure.Database.Constants;

namespace Bookmyslot.Api.SlotScheduler.Repositories.Queries
{
    public class CancelledSlotTableQueries
    {

        public const string InsertOrUpdateCustomerLastSharedSlotQuery = @"INSERT INTO" + " " + DatabaseConstants.CustomerLastSharedSlotTable + " " +
        @"(CreatedBy, Title, Country, TimeZone, SlotDate, SlotStartDateTimeUtc, SlotEndDateTimeUtc, SlotStartTime , SlotEndTime, ModifiedDateUtc) 
	   VALUES(@CreatedBy, @Title, @Country, @TimeZone,@SlotDate,@SlotStartDateTimeUtc,@SlotEndDateTimeUtc,  @SlotStartTime, @SlotEndTime, @ModifiedDateUtc) ON DUPLICATE KEY UPDATE
 Title=@Title, Country = @Country, TimeZone = @TimeZone, SlotDate = @SlotDate, SlotStartDateTimeUtc = @SlotStartDateTimeUtc, SlotEndDateTimeUtc = @SlotEndDateTimeUtc, SlotStartTime = @SlotStartTime, SlotEndTime = @SlotEndTime, ModifiedDateUtc = @ModifiedDateUtc ";


        public const string GetCustomerLastSharedSlotQuery = @"SELECT * FROM" + " " + DatabaseConstants.CustomerLastSharedSlotTable + " " + @"where CreatedBy = @CreatedBy";


        public const string CreateCancelledSlotQuery = @"INSERT INTO" + " " + DatabaseConstants.CancelledSlotTable + " " + @"(Id, Title, CreatedBy, CancelledBy, BookedBy, TimeZone, SlotDate, SlotStartDateTimeUtc,SlotEndDateTimeUtc, SlotStartTime, SlotEndTime,Country, CreatedDateUtc)
 VALUES(@Id, @Title, @CreatedBy, @CancelledBy, @BookedBy, @TimeZone, @SlotDate, @SlotStartDateTimeUtc,@SlotEndDateTimeUtc, @SlotStartTime, @SlotEndTime, @Country, @CreatedDateUtc); ";

        public const string GetCustomerSharedByCancelledSlotsQuery = @"SELECT * FROM" + " " + DatabaseConstants.CancelledSlotTable + " " + "where  CancelledBy=@CancelledBy  order by SlotStartDateTimeUtc Desc, SlotStartTime Desc";


        public const string GetCustomerBookedByCancelledSlotsQuery = @"SELECT * FROM" + " " + DatabaseConstants.CancelledSlotTable + " " + "where  CancelledBy=@CancelledBy  or BookedBy=@BookedBy order by SlotStartDateTimeUtc Desc, SlotStartTime Desc";
    }
}
