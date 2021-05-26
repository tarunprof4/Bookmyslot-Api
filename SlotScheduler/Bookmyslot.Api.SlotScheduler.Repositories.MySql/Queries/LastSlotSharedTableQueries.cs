using Bookmyslot.Api.Common.Contracts.Infrastructure.Database.Constants;

namespace Bookmyslot.Api.SlotScheduler.Repositories.Queries
{
    public class LastSlotSharedTableQueries
    {
        public const string InsertOrUpdateCustomerLastSharedSlotQuery = @"INSERT INTO" + " " + DatabaseConstants.CustomerLastSharedSlotTable + " " +
 @"(CreatedBy, Title, Country, TimeZone, SlotDate, SlotStartDateTimeUtc, SlotEndDateTimeUtc, SlotStartTime , SlotEndTime, ModifiedDateUtc) 
	   VALUES(@CreatedBy, @Title, @Country, @TimeZone,@SlotDate,@SlotStartDateTimeUtc,@SlotEndDateTimeUtc,  @SlotStartTime, @SlotEndTime, @ModifiedDateUtc) ON DUPLICATE KEY UPDATE
 Title=@Title, Country = @Country, TimeZone = @TimeZone, SlotDate = @SlotDate, SlotStartDateTimeUtc = @SlotStartDateTimeUtc, SlotEndDateTimeUtc = @SlotEndDateTimeUtc, SlotStartTime = @SlotStartTime, SlotEndTime = @SlotEndTime, ModifiedDateUtc = @ModifiedDateUtc ";

        public const string GetCustomerLastSharedSlotQuery = @"SELECT * FROM" + " " + DatabaseConstants.CustomerLastSharedSlotTable + " " + @"where CreatedBy = @CreatedBy";
    }
}
