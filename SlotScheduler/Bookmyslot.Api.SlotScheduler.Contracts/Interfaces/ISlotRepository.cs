using Bookmyslot.Api.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Contracts.Interfaces
{
    public interface ISlotRepository
    {
        Task<Response<SlotModel>> GetSlot(Guid id);
        Task<Response<IEnumerable<SlotModel>>> GetAllSlotsDateRange(DateTime startDate, DateTime endDate);
        Task<Response<Guid>> CreateSlot(SlotModel slot);
        Task<Response<bool>> UpdateSlot(SlotModel slot);
        Task<Response<bool>> DeleteSlot(Guid id);
    }
}
