using Bookmyslot.Api.SlotScheduler.Contracts;
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


    }

}
