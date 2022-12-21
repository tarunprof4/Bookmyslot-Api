using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces.Business;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces.Repository;
using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.SharedKernel.ValueObject;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Business
{
    public class CustomerLastSharedSlotBusiness : ICustomerLastSharedSlotBusiness
    {
        private readonly ICustomerLastSharedSlotRepository customerLastSharedSlotRepository;

        public CustomerLastSharedSlotBusiness(ICustomerLastSharedSlotRepository customerLastSharedSlotRepository)
        {
            this.customerLastSharedSlotRepository = customerLastSharedSlotRepository;
        }

        public async Task<Result<bool>> SaveCustomerLatestSharedSlot(CustomerLastSharedSlotModel customerLastSharedSlotModel)
        {
            return await this.customerLastSharedSlotRepository.SaveCustomerLatestSharedSlot(customerLastSharedSlotModel);
        }


        public async Task<Result<CustomerLastSharedSlotModel>> GetCustomerLatestSharedSlot(string customerId)
        {
            return await this.customerLastSharedSlotRepository.GetCustomerLatestSharedSlot(customerId);
        }
    }
}
