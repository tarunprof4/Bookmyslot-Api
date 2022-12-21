using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.SharedKernel.ValueObject;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Contracts.Interfaces
{
    public interface ISlotSchedulerBusiness
    {
        Task<Result<bool>> ScheduleSlot(SlotModel slotModel, string bookedBy);
    }
}
