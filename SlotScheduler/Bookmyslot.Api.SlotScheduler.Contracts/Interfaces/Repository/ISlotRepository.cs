using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.SharedKernel.ValueObject;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Contracts.Interfaces
{
    public interface ISlotRepository
    {
        Task<Result<SlotModel>> GetSlot(string slotId);
        Task<Result<string>> CreateSlot(SlotModel slotModel);
        Task<Result<bool>> UpdateSlotBooking(SlotModel slotModel);
        Task<Result<bool>> DeleteSlot(string slotId);
    }
}
