using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.SlotScheduler.Domain;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Contracts.Interfaces
{
    public interface ISlotRepository
    {
        Task<Response<SlotModel>> GetSlot(string slotId);
        Task<Response<string>> CreateSlot(SlotModel slotModel);
        Task<Response<bool>> UpdateSlotBooking(string slotId, string slotMeetingLink, string bookedBy);
        Task<Response<bool>> DeleteSlot(string slotId);
    }
}
