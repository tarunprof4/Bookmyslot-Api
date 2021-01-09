﻿using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Repositories.Enitites;
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
                TimeZone = slotEntity.TimeZone,
                SlotDate = slotEntity.SlotDate,
                SlotStartTime = slotEntity.SlotStartTime,
                SlotEndTime = slotEntity.SlotEndTime,
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



        internal static CancelledSlotModel CreateCancelledSlotModel(CancelledSlotEntity cancelledSlotEntity)
        {
            return new CancelledSlotModel()
            {
                Id = cancelledSlotEntity.Id,
                Title = cancelledSlotEntity.Title,
                CreatedBy = cancelledSlotEntity.CreatedBy,
                CancelledBy = cancelledSlotEntity.CancelledBy,
                TimeZone = cancelledSlotEntity.TimeZone,
                SlotDate = cancelledSlotEntity.SlotDate,
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
