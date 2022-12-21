using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.SharedKernel.ValueObject;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Contracts.Interfaces.Business
{
    public interface ICustomerLastSharedSlotBusiness
    {
        Task<Result<bool>> SaveCustomerLatestSharedSlot(CustomerLastSharedSlotModel customerLastSharedSlotModel);

        Task<Result<CustomerLastSharedSlotModel>> GetCustomerLatestSharedSlot(string customerId);
    }
}
