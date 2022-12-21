using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.SharedKernel.ValueObject;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Contracts.Interfaces.Repository
{

    public interface ICustomerLastSharedSlotRepository
    {
        Task<Result<bool>> SaveCustomerLatestSharedSlot(CustomerLastSharedSlotModel customerLastSharedSlotModel);

        Task<Result<CustomerLastSharedSlotModel>> GetCustomerLatestSharedSlot(string customerId);
    }
}
