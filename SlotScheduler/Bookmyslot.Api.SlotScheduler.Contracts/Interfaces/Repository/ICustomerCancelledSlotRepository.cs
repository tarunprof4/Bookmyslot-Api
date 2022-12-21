using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.SharedKernel.ValueObject;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Contracts.Interfaces
{
    public interface ICustomerCancelledSlotRepository
    {
        Task<Result<bool>> CreateCustomerCancelledSlot(CancelledSlotModel cancelledSlotModel);

        Task<Result<IEnumerable<CancelledSlotModel>>> GetCustomerSharedCancelledSlots(string customerId);

        Task<Result<IEnumerable<CancelledSlotModel>>> GetCustomerBookedCancelledSlots(string customerId);
    }
}
