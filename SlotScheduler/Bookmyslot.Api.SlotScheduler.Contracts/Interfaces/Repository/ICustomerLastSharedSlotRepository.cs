using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.SlotScheduler.Domain;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Contracts.Interfaces.Repository
{

    public interface ICustomerLastSharedSlotRepository
    {
        Task<Response<bool>> SaveCustomerLatestSharedSlot(CustomerLastSharedSlotModel customerLastSharedSlotModel);

        Task<Response<CustomerLastSharedSlotModel>> GetCustomerLatestSharedSlot(string customerId);
    }
}
