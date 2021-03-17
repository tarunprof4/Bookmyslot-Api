using Bookmyslot.Api.Common.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Contracts.Interfaces
{
    public interface ISlotRepository
    {
        Task<Response<SlotModel>> GetSlot(string slotId);
        Task<Response<IEnumerable<SlotModel>>> GetAllSlots(PageParameterModel pageParameterModel);
        Task<Response<string>> CreateSlot(SlotModel slotModel);
        Task<Response<bool>> UpdateBookedBySlot(string slotId, string bookedBy);
        Task<Response<bool>> DeleteSlot(string slotId);
    }
}
