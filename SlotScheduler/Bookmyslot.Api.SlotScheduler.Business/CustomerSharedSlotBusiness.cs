using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.Customers.Domain;
using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.SharedKernel;
using Bookmyslot.SharedKernel.ValueObject;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Contracts.Interfaces
{
    public class CustomerSharedSlotBusiness : ICustomerSharedSlotBusiness
    {
        private readonly ICustomerSharedSlotRepository customerSharedSlotRepository;
        private readonly ICustomerCancelledSlotRepository customerCancelledSlotRepository;
        private readonly ICustomerBusiness customerBusiness;
        public CustomerSharedSlotBusiness(ICustomerSharedSlotRepository customerSharedSlotRepository, ICustomerCancelledSlotRepository customerCancelledSlotRepository, ICustomerBusiness customerBusiness)
        {
            this.customerSharedSlotRepository = customerSharedSlotRepository;
            this.customerCancelledSlotRepository = customerCancelledSlotRepository;
            this.customerBusiness = customerBusiness;
        }


        public async Task<Result<SharedSlotModel>> GetCustomerYetToBeBookedSlots(string customerId)
        {
            var customerSlotModelsResponse = await this.customerSharedSlotRepository.GetCustomerYetToBeBookedSlots(customerId);

            if (customerSlotModelsResponse.ResultType == ResultType.Success)
            {
                var sharedSlotModel = new SharedSlotModel();
                sharedSlotModel.SharedSlotModels = new List<KeyValuePair<CustomerModel, SlotModel>>();
                foreach (var slotModel in customerSlotModelsResponse.Value)
                {
                    sharedSlotModel.SharedSlotModels.Add(new KeyValuePair<CustomerModel, SlotModel>(null, slotModel));
                }

                return new Result<SharedSlotModel>() { Value = sharedSlotModel };
            }

            return Result<SharedSlotModel>.Empty(new List<string>() { AppBusinessMessagesConstants.NoSlotsFound });
        }


        public async Task<Result<IEnumerable<CancelledSlotModel>>> GetCustomerCancelledSlots(string customerId)
        {
            return await this.customerCancelledSlotRepository.GetCustomerSharedCancelledSlots(customerId);
        }



        public async Task<Result<SharedSlotModel>> GetCustomerBookedSlots(string customerId)
        {
            var customerSlotModelsResponse = await this.customerSharedSlotRepository.GetCustomerBookedSlots(customerId);

            if (customerSlotModelsResponse.ResultType == ResultType.Success)
            {
                return await GetCustomerBookedOrCompletedSlots(customerSlotModelsResponse);
            }

            return Result<SharedSlotModel>.Empty(new List<string>() { AppBusinessMessagesConstants.NoSlotsFound });
        }



        public async Task<Result<SharedSlotModel>> GetCustomerCompletedSlots(string customerId)
        {
            var customerSlotModelsResponse = await this.customerSharedSlotRepository.GetCustomerCompletedSlots(customerId);

            if (customerSlotModelsResponse.ResultType == ResultType.Success)
            {
                return await GetCustomerBookedOrCompletedSlots(customerSlotModelsResponse);
            }

            return Result<SharedSlotModel>.Empty(new List<string>() { AppBusinessMessagesConstants.NoSlotsFound });
        }


        private async Task<Result<SharedSlotModel>> GetCustomerBookedOrCompletedSlots(Result<IEnumerable<SlotModel>> customerSlotModelsResponse)
        {
            var customerIds = customerSlotModelsResponse.Value.Select(a => a.BookedBy);
            var customerModelsResponse = await this.customerBusiness.GetCustomersByCustomerIds(customerIds);

            var sharedSlotModel = new SharedSlotModel();
            sharedSlotModel.SharedSlotModels = new List<KeyValuePair<CustomerModel, SlotModel>>();
            foreach (var slotModel in customerSlotModelsResponse.Value)
            {
                var bookedByCustomerModel = customerModelsResponse.Value.First(a => a.Id == slotModel.BookedBy);
                sharedSlotModel.SharedSlotModels.Add(new KeyValuePair<CustomerModel, SlotModel>(bookedByCustomerModel, slotModel));
            }

            return new Result<SharedSlotModel>() { Value = sharedSlotModel };
        }
    }
}
