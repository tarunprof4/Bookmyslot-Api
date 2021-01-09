using Bookmyslot.Api.Common.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Contracts.Interfaces
{
    public interface ICustomerCancelledSlotRepository
    {
        Task<Response<bool>> CreateCustomerCancelledSlot(CancelledSlotModel cancelledSlotModel);

        Task<Response<IEnumerable<CancelledSlotModel>>> GetCustomerCancelledSlots(string customerId);
    }
}
