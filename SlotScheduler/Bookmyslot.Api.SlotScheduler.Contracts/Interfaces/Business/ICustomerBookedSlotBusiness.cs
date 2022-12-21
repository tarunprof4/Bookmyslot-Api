using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.SharedKernel.ValueObject;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Contracts.Interfaces
{
    public interface ICustomerBookedSlotBusiness
    {
        Task<Result<BookedSlotModel>> GetCustomerBookedSlots(string customerId);

        Task<Result<BookedSlotModel>> GetCustomerCompletedSlots(string customerId);

        Task<Result<IEnumerable<CancelledSlotInformationModel>>> GetCustomerCancelledSlots(string customerId);
    }
}
