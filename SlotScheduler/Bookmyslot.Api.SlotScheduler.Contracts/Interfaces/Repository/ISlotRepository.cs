using Bookmyslot.Api.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Contracts.Interfaces
{
    public interface ISlotRepository
    {
        Task<Response<SlotModel>> GetSlot(Guid slotId);
        Task<Response<IEnumerable<SlotModel>>> GetAllSlots(PageParameterModel pageParameterModel);
        Task<Response<Guid>> CreateSlot(SlotModel slotModel);
        Task<Response<bool>> UpdateSlot(Guid slotId, string bookedBy);
        Task<Response<bool>> DeleteSlot(Guid slotId);
    }
}
