using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Helpers;
using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.Api.SlotScheduler.Repositories.Enitites;
using System;
using System.Collections.Generic;

namespace Bookmyslot.Api.SlotScheduler.Repositories.ModelFactory
{
    internal class ModelFactory
    {
        internal static SlotModel CreateSlotModel(SlotEntity slotEntity)
        {
            return new SlotModel()
            {
                Id = slotEntity.Id,
                Title = slotEntity.Title,
                CreatedBy = slotEntity.CreatedBy,
                BookedBy = slotEntity.BookedBy,
                Country = slotEntity.Country,
                SlotStartZonedDateTime = NodaTimeHelper.ConvertDateStringToZonedDateTime(slotEntity.SlotDate, DateTimeConstants.ApplicationDatePattern, slotEntity.SlotStartTime, slotEntity.TimeZone),
                SlotStartTime = slotEntity.SlotStartTime,
                SlotEndTime = slotEntity.SlotEndTime,
                SlotMeetingLink = slotEntity.SlotMeetingLink,
                CreatedDateUtc = DateTime.SpecifyKind(slotEntity.CreatedDateUtc, DateTimeKind.Utc)
            };
        }

        internal static CustomerLastSharedSlotModel CreateCustomerLastSharedSlotModel(CustomerLastSharedSlotEntity customerLastSharedSlotEntity)
        {
            return new CustomerLastSharedSlotModel()
            {
                CreatedBy = customerLastSharedSlotEntity.CreatedBy,
                Title = customerLastSharedSlotEntity.Title,
                Country = customerLastSharedSlotEntity.Country,
                TimeZone = customerLastSharedSlotEntity.TimeZone,
                SlotStartZonedDateTime = NodaTimeHelper.ConvertDateStringToZonedDateTime(customerLastSharedSlotEntity.SlotDate, DateTimeConstants.ApplicationDatePattern, customerLastSharedSlotEntity.SlotStartTime, customerLastSharedSlotEntity.TimeZone),
                SlotStartTime = customerLastSharedSlotEntity.SlotStartTime,
                SlotEndTime = customerLastSharedSlotEntity.SlotEndTime,
            };
        }




        internal static List<SlotModel> CreateSlotModels(IEnumerable<SlotEntity> slotEntities)
        {
            List<SlotModel> slotModels = new List<SlotModel>();
            foreach (var slotEntity in slotEntities)
            {
                slotModels.Add(CreateSlotModel(slotEntity));
            }
            return slotModels;
        }


        internal static List<string> CreateCustomersFromSlotModels(IEnumerable<SlotEntity> slotEntities)
        {
            List<string> customers = new List<string>();
            foreach (var slotEntity in slotEntities)
            {
                customers.Add(slotEntity.CreatedBy);
            }
            return customers;
        }



        internal static CancelledSlotModel CreateCancelledSlotModel(CancelledSlotEntity cancelledSlotEntity)
        {
            return new CancelledSlotModel()
            {
                Id = cancelledSlotEntity.Id,
                Title = cancelledSlotEntity.Title,
                CreatedBy = cancelledSlotEntity.CreatedBy,
                CancelledBy = cancelledSlotEntity.CancelledBy,
                BookedBy = cancelledSlotEntity.BookedBy,
                Country = cancelledSlotEntity.Country,
                SlotStartZonedDateTime = NodaTimeHelper.ConvertDateStringToZonedDateTime(cancelledSlotEntity.SlotDate, DateTimeConstants.ApplicationDatePattern, cancelledSlotEntity.SlotStartTime, cancelledSlotEntity.TimeZone),
                SlotStartTime = cancelledSlotEntity.SlotStartTime,
                SlotEndTime = cancelledSlotEntity.SlotEndTime,
            };
        }

        internal static List<CancelledSlotModel> CreateCancelledSlotModels(IEnumerable<CancelledSlotEntity> cancelledSlotlotEntities)
        {
            var cancelledSlotModels = new List<CancelledSlotModel>();
            foreach (var cancelledSlotlotEntity in cancelledSlotlotEntities)
            {
                cancelledSlotModels.Add(CreateCancelledSlotModel(cancelledSlotlotEntity));
            }
            return cancelledSlotModels;
        }


    }

}
