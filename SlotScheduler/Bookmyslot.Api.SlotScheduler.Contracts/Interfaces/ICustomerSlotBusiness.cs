using Bookmyslot.Api.Common.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Contracts.Interfaces
{
    public interface ICustomerSlotBusiness
    {
        Task<Response<List<CustomerSlotModel>>> GetDistinctCustomersNearestSlotFromToday(PageParameterModel pageParameterModel);

        Task<Response<CustomerSlotModel>> GetCustomerAvailableSlots(PageParameterModel pageParameterModel, string email);
    }
}
