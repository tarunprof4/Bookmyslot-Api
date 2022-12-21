using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.SharedKernel.ValueObject;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Contracts.Interfaces
{
    public interface ICustomerSlotRepository
    {
        Task<Result<IEnumerable<string>>> GetDistinctCustomersNearestSlotFromToday(PageParameter pageParameterModel);

        Task<Result<IEnumerable<SlotModel>>> GetCustomerAvailableSlots(PageParameter pageParameterModel, string email);

    }
}
