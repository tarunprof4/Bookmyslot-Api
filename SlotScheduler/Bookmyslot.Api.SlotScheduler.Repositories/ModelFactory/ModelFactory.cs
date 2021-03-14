﻿using Bookmyslot.Api.Common.Helpers;
using Bookmyslot.Api.SlotScheduler.Contracts;
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
                SlotZonedDate = NodaTimeHelper.ConvertUtcDateTimeToZonedDateTime(DateTime.SpecifyKind(slotEntity.SlotDateUtc, DateTimeKind.Utc), slotEntity.TimeZone),
                SlotStartTime = slotEntity.SlotStartTime,
                SlotEndTime = slotEntity.SlotEndTime,
                CreatedDateUtc = DateTime.SpecifyKind(slotEntity.CreatedDateUtc, DateTimeKind.Utc)
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
                SlotZonedDate = NodaTimeHelper.ConvertUtcDateTimeToZonedDateTime(DateTime.SpecifyKind(cancelledSlotEntity.SlotDateUtc, DateTimeKind.Utc), cancelledSlotEntity.TimeZone),
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
