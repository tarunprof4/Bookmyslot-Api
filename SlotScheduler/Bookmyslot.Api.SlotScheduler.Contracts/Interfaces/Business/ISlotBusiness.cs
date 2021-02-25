using Bookmyslot.Api.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Contracts.Interfaces
{
    public interface ISlotBusiness
    {
        Task<Response<SlotModel>> GetSlot(Guid slotId);

        Task<Response<IEnumerable<SlotModel>>> GetAllSlots(PageParameterModel pageParameterModel);

        Task<Response<Guid>> CreateSlot(SlotModel slot,string slotDateString, string createdBy);
        Task<Response<bool>> CancelSlot(Guid slotId, string deletedBy);
    }
}
