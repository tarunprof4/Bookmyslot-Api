using Bookmyslot.Api.Common.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Contracts.Interfaces
{
    public interface ICustomerBookedSlotBusiness
    {
        Task<Response<BookedSlotModel>> GetCustomerBookedSlots(string customerId);

        Task<Response<BookedSlotModel>> GetCustomerCompletedSlots(string customerId);

        Task<Response<IEnumerable<CancelledSlotInformationModel>>> GetCustomerCancelledSlots(string customerId);
    }
}
