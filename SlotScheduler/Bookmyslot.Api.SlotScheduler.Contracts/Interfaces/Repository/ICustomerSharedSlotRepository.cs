using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.SharedKernel.ValueObject;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Contracts.Interfaces
{
    public interface ICustomerSharedSlotRepository
    {
        Task<Result<IEnumerable<SlotModel>>> GetCustomerYetToBeBookedSlots(string customerId);

        Task<Result<IEnumerable<SlotModel>>> GetCustomerBookedSlots(string customerId);

        Task<Result<IEnumerable<SlotModel>>> GetCustomerCompletedSlots(string customerId);

    }
}
