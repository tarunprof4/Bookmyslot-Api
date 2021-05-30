using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.SlotScheduler.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Contracts.Interfaces
{
    public interface ICustomerSharedSlotBusiness
    {
        Task<Response<SharedSlotModel>> GetCustomerYetToBeBookedSlots(string customerId);

        Task<Response<SharedSlotModel>> GetCustomerBookedSlots(string customerId);

        Task<Response<SharedSlotModel>> GetCustomerCompletedSlots(string customerId);

        Task<Response<IEnumerable<CancelledSlotModel>>> GetCustomerCancelledSlots(string customerId);
    }
}
