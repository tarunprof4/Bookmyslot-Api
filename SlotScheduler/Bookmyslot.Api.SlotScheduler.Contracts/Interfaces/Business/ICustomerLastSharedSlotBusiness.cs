using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.SlotScheduler.Domain;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Contracts.Interfaces.Business
{
    public interface ICustomerLastSharedSlotBusiness
    {
        Task<Response<bool>> SaveCustomerLatestSharedSlot(CustomerLastSharedSlotModel customerLastSharedSlotModel);

        Task<Response<CustomerLastSharedSlotModel>> GetCustomerLatestSharedSlot(string customerId);
    }
}
