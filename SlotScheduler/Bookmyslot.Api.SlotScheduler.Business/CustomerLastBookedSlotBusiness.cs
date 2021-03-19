using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces.Business;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces.Repository;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Business
{
    public class CustomerLastBookedSlotBusiness : ICustomerLastBookedSlotBusiness
    {
        private readonly ICustomerLastBookedSlotRepository customerLastBookedSlotRepository;

        public CustomerLastBookedSlotBusiness(ICustomerLastBookedSlotRepository customerLastBookedSlotRepository)
        {
            this.customerLastBookedSlotRepository = customerLastBookedSlotRepository;
        }

        public async Task<Response<bool>> SaveCustomerLatestSlot(CustomerLastBookedSlotModel customerLastBookedSlotModel)
        {
            return await this.customerLastBookedSlotRepository.SaveCustomerLatestSlot(customerLastBookedSlotModel);
        }


        public async Task<Response<CustomerLastBookedSlotModel>> GetCustomerLatestSlot(string customerId)
        {
            return await this.customerLastBookedSlotRepository.GetCustomerLatestSlot(customerId);
        }
    }
}
