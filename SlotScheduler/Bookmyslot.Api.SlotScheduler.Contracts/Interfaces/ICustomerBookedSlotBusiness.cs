using Bookmyslot.Api.Common.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Contracts.Interfaces
{
    public interface ICustomerBookedSlotBusiness
    {
        Task<Response<IEnumerable<BookedSlotModel>>> GetCustomerBookedSlots();

        Task<Response<IEnumerable<BookedSlotModel>>> GetCustomerCompletedSlots();

        Task<Response<IEnumerable<CancelledSlotModel>>> GetCustomerCancelledSlots();
    }
}
