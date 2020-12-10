using Bookmyslot.Api.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Contracts.Interfaces
{
    public interface ISlotRepository
    {
        Task<Response<SlotModel>> GetSlot(Guid slotId);
        Task<Response<IEnumerable<SlotModel>>> GetAllSlotsDateRange(DateTime startDate, DateTime endDate);
        Task<Response<Guid>> CreateSlot(SlotModel slotModel);
        Task<Response<bool>> UpdateSlot(SlotModel slotModel);
        Task<Response<bool>> DeleteSlot(SlotModel slotModel);
    }
}
