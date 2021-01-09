using Bookmyslot.Api.Common.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Contracts.Interfaces
{
    public interface ICustomerSharedSlotBusiness
    {
        Task<Response<IEnumerable<SharedSlotModel>>> GetCustomerYetToBeBookedSlots();

        Task<Response<IEnumerable<SharedSlotModel>>> GetCustomerBookedSlots();

        Task<Response<IEnumerable<SharedSlotModel>>> GetCustomerCompletedSlots();

        Task<Response<IEnumerable<SharedSlotModel>>> GetCustomerCancelledSlots();
    }
}
