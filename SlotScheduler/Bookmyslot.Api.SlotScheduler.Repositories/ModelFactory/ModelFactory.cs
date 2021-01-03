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
               TimeZone = slotEntity.TimeZone,
               SlotDate = slotEntity.SlotDate,
               StartTime = slotEntity.StartTime,
               EndTime = slotEntity.EndTime,
            };
        }


        internal static List<SlotModel> CreateCustomerModels(IEnumerable<SlotEntity> slotEntities)
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
