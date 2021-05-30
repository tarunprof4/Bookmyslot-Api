using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Helpers;
using Bookmyslot.Api.SlotScheduler.Domain;
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
                TimeZone = slotModel.SlotStartZonedDateTime.Zone.Id,
                SlotDate = NodaTimeHelper.FormatLocalDate(slotModel.SlotStartZonedDateTime.Date, DateTimeConstants.ApplicationDatePattern),
                SlotStartDateTimeUtc = NodaTimeHelper.ConvertZonedDateTimeToUtcDateTime(slotModel.SlotStartZonedDateTime),
                SlotEndDateTimeUtc = (NodaTimeHelper.ConvertZonedDateTimeToUtcDateTime(slotModel.SlotStartZonedDateTime)).Add(slotModel.SlotDuration),
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
                TimeZone = customerLastSharedSlotModel.SlotStartZonedDateTime.Zone.Id,
                SlotDate = NodaTimeHelper.FormatLocalDate(customerLastSharedSlotModel.SlotStartZonedDateTime.Date, DateTimeConstants.ApplicationDatePattern),
                SlotStartDateTimeUtc = NodaTimeHelper.ConvertZonedDateTimeToUtcDateTime(customerLastSharedSlotModel.SlotStartZonedDateTime),
                SlotEndDateTimeUtc = (NodaTimeHelper.ConvertZonedDateTimeToUtcDateTime(customerLastSharedSlotModel.SlotStartZonedDateTime)).Add(customerLastSharedSlotModel.SlotDuration),
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
                TimeZone = cancelledSlotModel.SlotStartZonedDateTime.Zone.Id,
                SlotStartDateTimeUtc = NodaTimeHelper.ConvertZonedDateTimeToUtcDateTime(cancelledSlotModel.SlotStartZonedDateTime),
                SlotEndDateTimeUtc = (NodaTimeHelper.ConvertZonedDateTimeToUtcDateTime(cancelledSlotModel.SlotStartZonedDateTime)).Add(cancelledSlotModel.SlotDuration),
                SlotDate = NodaTimeHelper.FormatLocalDate(cancelledSlotModel.SlotStartZonedDateTime.Date, DateTimeConstants.ApplicationDatePattern),
                SlotStartTime = cancelledSlotModel.SlotStartTime,
                SlotEndTime = cancelledSlotModel.SlotEndTime,
                CreatedDateUtc = DateTime.UtcNow
            };
        }

    }
}
