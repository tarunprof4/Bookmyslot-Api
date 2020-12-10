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
                StartTime = slotModel.StartTime,
                EndTime = slotModel.EndTime,
            };
        }

        internal static SlotEntity UpdateSlotEntity(SlotModel slotModel)
        {
            var slotEntity = CreateSlotEntity(slotModel);
            slotEntity.Id = slotModel.Id;
            slotEntity.ModifiedDate = DateTime.UtcNow;
            return slotEntity;
        }

        internal static SlotEntity DeleteSlotEntity(SlotModel slotModel)
        {
            var slotEntity = UpdateSlotEntity(slotModel);
            slotEntity.IsDeleted = true;
            return slotEntity;
        }
    }
}
