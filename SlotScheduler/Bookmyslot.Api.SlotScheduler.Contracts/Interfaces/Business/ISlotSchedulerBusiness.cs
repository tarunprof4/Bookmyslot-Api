using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Customers.Domain;
using Bookmyslot.Api.SlotScheduler.Domain;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Contracts.Interfaces
{
    public interface ISlotSchedulerBusiness
    {
        Task<Response<bool>> ScheduleSlot(SlotModel slotModel, string bookedByCustomerSummaryModel);
    }
}
