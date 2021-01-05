using Bookmyslot.Api.Common.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Contracts.Interfaces
{
    public interface ICustomerSlotBusiness
    {
        Task<Response<List<CustomerSlotModel>>> GetDistinctCustomersLatestSlot(PageParameterModel pageParameterModel);

        Task<Response<List<CustomerSlotModel>>> GetCustomerSlots(PageParameterModel pageParameterModel, string email);
    }
}
