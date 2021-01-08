using Bookmyslot.Api.Common.Contracts;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Contracts.Interfaces
{
    public interface ISlotSchedulerBusiness
    {
        Task<Response<bool>> ScheduleSlot(SlotModel slotModel);
    }
}
