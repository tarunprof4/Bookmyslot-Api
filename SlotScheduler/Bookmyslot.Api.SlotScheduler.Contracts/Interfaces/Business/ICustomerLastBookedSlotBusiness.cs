using Bookmyslot.Api.Common.Contracts;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Contracts.Interfaces.Business
{
    public interface ICustomerLastBookedSlotBusiness
    {
        Task<Response<bool>> SaveCustomerLatestSlot(CustomerLastBookedSlotModel customerLastBookedSlotModel);

        Task<Response<CustomerLastBookedSlotModel>> GetCustomerLatestSlot(string customerId);
    }
}
