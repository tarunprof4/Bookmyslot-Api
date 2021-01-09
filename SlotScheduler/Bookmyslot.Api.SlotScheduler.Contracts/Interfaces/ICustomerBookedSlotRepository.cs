using Bookmyslot.Api.Common.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Contracts.Interfaces
{
    public interface ICustomerBookedSlotRepository
    {
        Task<Response<IEnumerable<SlotModel>>> GetCustomerBookedSlots(string customerId);

        Task<Response<IEnumerable<SlotModel>>> GetCustomerCompletedSlots(string customerId);
    }
}
