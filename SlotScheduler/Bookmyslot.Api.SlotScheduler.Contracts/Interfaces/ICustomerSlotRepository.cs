using Bookmyslot.Api.Common.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Contracts.Interfaces
{
    public interface ICustomerSlotRepository
    {
        Task<Response<IEnumerable<SlotModel>>> GetDistinctCustomersNearestSlotFromToday(PageParameterModel pageParameterModel);

        Task<Response<IEnumerable<SlotModel>>> GetCustomerSlots(PageParameterModel pageParameterModel, string email);
    }
}
