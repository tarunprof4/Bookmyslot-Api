using Bookmyslot.Api.Common.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Contracts.Interfaces
{
    public interface ICustomerSharedSlotBusiness
    {
        Task<Response<IEnumerable<SharedSlotModel>>> GetCustomerYetToBeBookedSlots(string customerId);

        Task<Response<IEnumerable<SharedSlotModel>>> GetCustomerBookedSlots(string customerId);

        Task<Response<IEnumerable<SharedSlotModel>>> GetCustomerCompletedSlots(string customerId);

        Task<Response<IEnumerable<SharedSlotModel>>> GetCustomerCancelledSlots(string customerId);
    }
}
