using Bookmyslot.Api.Common.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Contracts.Interfaces
{
    public interface ISlotBusiness
    {
        Task<Response<SlotModel>> GetSlot(string slotId);

        Task<Response<string>> CreateSlot(SlotModel slot, string createdBy);
        Task<Response<bool>> CancelSlot(string slotId, string deletedBy);
    }
}
