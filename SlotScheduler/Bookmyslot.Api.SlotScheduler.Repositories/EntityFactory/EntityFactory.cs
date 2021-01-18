using Bookmyslot.Api.Common.Contracts.ExtensionMethods;
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
                Id = Guid.NewGuid(),
                Title = slotModel.Title,
                CreatedBy = slotModel.CreatedBy,
                BookedBy = slotModel.BookedBy,
                TimeZone = slotModel.TimeZone,
                SlotDateUtc = slotModel.SlotDateUtc,
                SlotStartTime = slotModel.SlotStartTime,
                SlotEndTime = slotModel.SlotEndTime,
            };
        }

        internal static SlotEntity UpdateSlotEntity(SlotModel slotModel)
        {
            var slotEntity = CreateSlotEntity(slotModel);
            slotEntity.Id = slotModel.Id;
            slotEntity.ModifiedDateUtc = DateTime.UtcNow;
            return slotEntity;
        }

        internal static SlotEntity DeleteSlotEntity(SlotModel slotModel)
        {
            var slotEntity = UpdateSlotEntity(slotModel);
            slotEntity.IsDeleted = true;
            return slotEntity;
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
                TimeZone = cancelledSlotModel.TimeZone,
                SlotDateUtc = cancelledSlotModel.SlotDateUtc,
                SlotStartTime = cancelledSlotModel.SlotStartTime,
                SlotEndTime = cancelledSlotModel.SlotEndTime,
                CreatedDateUtc = DateTime.UtcNow
            };
        }

    }
}
