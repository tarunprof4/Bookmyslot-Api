using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
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


        public async Task<Response<IEnumerable<SharedSlotModel>>> GetCustomerYetToBeBookedSlots(string customerId)
        {
            var customerSlotModelsResponse = await this.customerSharedSlotRepository.GetCustomerYetToBeBookedSlots(customerId);

            if (customerSlotModelsResponse.ResultType == ResultType.Success)
            {
                var sharedSlotModels = new List<SharedSlotModel>();
                foreach (var slotModel in customerSlotModelsResponse.Result)
                {
                    sharedSlotModels.Add(new SharedSlotModel() { SlotModel = slotModel });
                }

                return new Response<IEnumerable<SharedSlotModel>>() { Result = sharedSlotModels };
            }

            return Response<IEnumerable<SharedSlotModel>>.Empty(new List<string>() { AppBusinessMessagesConstants.NoSlotsFound });
        }


        public async Task<Response<IEnumerable<CancelledSlotModel>>> GetCustomerCancelledSlots(string customerId)
        {
            return await this.customerCancelledSlotRepository.GetCustomerSharedCancelledSlots(customerId);
        }

        

        public async Task<Response<IEnumerable<SharedSlotModel>>> GetCustomerBookedSlots(string customerId)
        {
            var customerSlotModelsResponse = await this.customerSharedSlotRepository.GetCustomerBookedSlots(customerId);

            if (customerSlotModelsResponse.ResultType == ResultType.Success)
            {
                return await GetCustomerBookedOrCompletedSlots(customerSlotModelsResponse);
            }

            return Response<IEnumerable<SharedSlotModel>>.Empty(new List<string>() { AppBusinessMessagesConstants.NoSlotsFound });
        }

        

        public async Task<Response<IEnumerable<SharedSlotModel>>> GetCustomerCompletedSlots(string customerId)
        {
            var customerSlotModelsResponse = await this.customerSharedSlotRepository.GetCustomerCompletedSlots(customerId);

            if (customerSlotModelsResponse.ResultType == ResultType.Success)
            {
                return await GetCustomerBookedOrCompletedSlots(customerSlotModelsResponse);
            }

            return Response<IEnumerable<SharedSlotModel>>.Empty(new List<string>() { AppBusinessMessagesConstants.NoSlotsFound });
        }


        private async Task<Response<IEnumerable<SharedSlotModel>>> GetCustomerBookedOrCompletedSlots(Response<IEnumerable<SlotModel>> customerSlotModelsResponse)
        {
            var customerIds = customerSlotModelsResponse.Result.Select(a => a.BookedBy);
            var customerModelsResponse = await this.customerBusiness.GetCustomersByCustomerIds(customerIds);

            var sharedSlotModels = new List<SharedSlotModel>();
            foreach (var slotModel in customerSlotModelsResponse.Result)
            {
                var sharedSlotModel = new SharedSlotModel();
                sharedSlotModel.SlotModel = slotModel;
                sharedSlotModel.BookedByCustomerModel = customerModelsResponse.Result.First(a => a.Id == slotModel.BookedBy);

                sharedSlotModels.Add(sharedSlotModel);
            }

            return new Response<IEnumerable<SharedSlotModel>>() { Result = sharedSlotModels };
        }
    }
}
