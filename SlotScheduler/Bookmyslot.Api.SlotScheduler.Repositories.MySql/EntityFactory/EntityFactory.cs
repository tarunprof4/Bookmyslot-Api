using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Helpers;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Repositories.Enitites;
using System;

namespace Bookmyslot.Api.SlotScheduler.Repositories.EntityFactory
{
    internal class EntityFactory
    {
        internal static SlotEntity CreateSlotEntity(SlotModel slotModel)
        {
            return new SlotEntity()
            {
                Id = Guid.NewGuid().ToString(),
                Title = slotModel.Title,
                CreatedBy = slotModel.CreatedBy,
                BookedBy = slotModel.BookedBy,
                Country = slotModel.Country,
                TimeZone = slotModel.SlotZonedDate.Zone.Id,
                SlotDate = NodaTimeHelper.FormatLocalDate(slotModel.SlotZonedDate.Date, DateTimeConstants.ApplicationDatePattern),
                SlotDateUtc = NodaTimeHelper.ConvertZonedDateTimeToUtcDateTime(slotModel.SlotZonedDate),
                SlotStartTime = slotModel.SlotStartTime,
                SlotEndTime = slotModel.SlotEndTime,
                CreatedDateUtc = DateTime.UtcNow
            };
        }


        internal static CustomerLastSharedSlotEntity CreateCustomerLastSharedSlotEntity(CustomerLastSharedSlotModel customerLastSharedSlotModel)
        {
            return new CustomerLastSharedSlotEntity()
            {
                CreatedBy = customerLastSharedSlotModel.CreatedBy,
                Title = customerLastSharedSlotModel.Title,
                Country = customerLastSharedSlotModel.Country,
                TimeZone = customerLastSharedSlotModel.SlotZonedDate.Zone.Id,
                SlotDate = NodaTimeHelper.FormatLocalDate(customerLastSharedSlotModel.SlotZonedDate.Date, DateTimeConstants.ApplicationDatePattern),
                SlotDateUtc = NodaTimeHelper.ConvertZonedDateTimeToUtcDateTime(customerLastSharedSlotModel.SlotZonedDate),
                SlotStartTime = customerLastSharedSlotModel.SlotStartTime,
                SlotEndTime = customerLastSharedSlotModel.SlotEndTime,
                ModifiedDateUtc = DateTime.UtcNow
            };
        }


        internal static CancelledSlotEntity CreateCancelledSlotEntity(CancelledSlotModel cancelledSlotModel)
        {
            return new CancelledSlotEntity()
            {
                Id = cancelledSlotModel.Id,
                Title = cancelledSlotModel.Title,
                CreatedBy = cancelledSlotModel.CreatedBy,
                CancelledBy = cancelledSlotModel.CancelledBy,
                BookedBy = cancelledSlotModel.BookedBy,
                Country = cancelledSlotModel.Country,
                TimeZone = cancelledSlotModel.SlotZonedDate.Zone.Id,
                SlotDateUtc = NodaTimeHelper.ConvertZonedDateTimeToUtcDateTime(cancelledSlotModel.SlotZonedDate),
                SlotDate = NodaTimeHelper.FormatLocalDate(cancelledSlotModel.SlotZonedDate.Date, DateTimeConstants.ApplicationDatePattern),
                SlotStartTime = cancelledSlotModel.SlotStartTime,
                SlotEndTime = cancelledSlotModel.SlotEndTime,
                CreatedDateUtc = DateTime.UtcNow
            };
        }

    }
}
