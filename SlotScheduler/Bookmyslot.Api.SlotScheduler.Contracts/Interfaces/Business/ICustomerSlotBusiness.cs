using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.SlotScheduler.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Contracts.Interfaces
{
    public interface ICustomerSlotBusiness
    {
        Task<Response<List<CustomerSlotModel>>> GetDistinctCustomersNearestSlotFromToday(PageParameterModel pageParameterModel);

        Task<Response<BookAvailableSlotModel>> GetCustomerAvailableSlots(PageParameterModel pageParameterModel, string customerId, string createdBy);
    }
}
