using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.SharedKernel.ValueObject;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Contracts.Interfaces
{
    public interface ICustomerSharedSlotBusiness
    {
        Task<Result<SharedSlotModel>> GetCustomerYetToBeBookedSlots(string customerId);

        Task<Result<SharedSlotModel>> GetCustomerBookedSlots(string customerId);

        Task<Result<SharedSlotModel>> GetCustomerCompletedSlots(string customerId);

        Task<Result<IEnumerable<CancelledSlotModel>>> GetCustomerCancelledSlots(string customerId);
    }
}
