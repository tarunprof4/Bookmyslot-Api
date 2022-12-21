using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.SharedKernel.ValueObject;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Contracts.Interfaces
{
    public interface ISlotBusiness
    {
        Task<Result<SlotModel>> GetSlot(string slotId);

        Task<Result<string>> CreateSlot(SlotModel slot, string createdBy);
        Task<Result<bool>> CancelSlot(string slotId, string cancelledBy);
    }
}
