using System;
using System.Collections.Generic;
using System.Text;

namespace Bookmyslot.Api.SlotScheduler.Repositories.Queries
{
    
    public class SlotTableQueries
    {

        public const string GetDistinctCustomersNearestSlotFromTodayQuery = @"select * from (
SELECT id, title, CreatedBy, SlotStartTime, SlotEndTime, IsDeleted, ModifiedDate, TimeZone, SlotDate,
       ROW_NUMBER() OVER(PARTITION BY CreatedBy ORDER BY slotDate ASC) AS RowNumber
FROM Slot where IsDeleted = @IsDeleted  and SlotDate > GETDATE()
)  as resultSet where resultSet.RowNumber = 1 order by resultSet.Id ASC OFFSET @PageNumber ROWS  FETCH Next @PageSize ROWS ONLY";


        public const string GetCustomerAvailableSlotsFromTodayQuery = @"SELECT * FROM Slot where IsDeleted=@IsDeleted and CreatedBy= @CreatedBy and SlotDate > GETDATE() order by SlotDate, SlotStartTime
   OFFSET @PageNumber ROWS FETCH Next @PageSize ROWS ONLY";


    }
}
