using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.SharedKernel.ValueObject;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Contracts.Interfaces
{
    public interface ICustomerSlotBusiness
    {
        Task<Result<List<CustomerSlotModel>>> GetDistinctCustomersNearestSlotFromToday(PageParameter pageParameterModel);

        Task<Result<BookAvailableSlotModel>> GetCustomerAvailableSlots(PageParameter pageParameterModel, string customerId, string createdBy);
    }
}
