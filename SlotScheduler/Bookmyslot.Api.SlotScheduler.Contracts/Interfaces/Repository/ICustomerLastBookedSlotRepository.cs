using Bookmyslot.Api.Common.Contracts;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Contracts.Interfaces.Repository
{

    public interface ICustomerLastBookedSlotRepository
    {
        Task<Response<bool>> SaveCustomerLatestSlot(CustomerLastBookedSlotModel customerLastBookedSlotModel);

        Task<Response<CustomerLastBookedSlotModel>> GetCustomerLatestSlot(string customerId);
    }
}
