using Bookmyslot.Api.Common;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Contracts.Interfaces
{
    public class CustomerBookedSlotBusiness : ICustomerBookedSlotBusiness
    {
        private readonly ICustomerBookedSlotRepository customerBookedSlotRepository;
        private readonly ICustomerCancelledSlotRepository customerCancelledSlotRepository;
        private readonly ICustomerBusiness customerBusiness;
        public CustomerBookedSlotBusiness(ICustomerBookedSlotRepository customerBookedSlotRepository, ICustomerCancelledSlotRepository customerCancelledSlotRepository, ICustomerBusiness customerBusiness)
        {
            this.customerBookedSlotRepository = customerBookedSlotRepository;
            this.customerCancelledSlotRepository = customerCancelledSlotRepository;
            this.customerBusiness = customerBusiness;
        }

        public async Task<Response<IEnumerable<CancelledSlotModel>>> GetCustomerCancelledSlots()
        {
            var customerId = UserService.GetUser();
            return await this.customerCancelledSlotRepository.GetCustomerCancelledSlots(customerId);
        }

        public async Task<Response<IEnumerable<BookedSlotModel>>> GetCustomerBookedSlots()
        {
            var customerId = UserService.GetUser();
            var customerSlotModelsResponse = await this.customerBookedSlotRepository.GetCustomerBookedSlots(customerId);

            if (customerSlotModelsResponse.ResultType == ResultType.Success)
            {
                return await GetCustomerSlots(customerSlotModelsResponse);
            }

            return Response<IEnumerable<BookedSlotModel>>.Empty(new List<string>() { AppBusinessMessages.NoSlotsFound });
        }

        public async Task<Response<IEnumerable<BookedSlotModel>>> GetCustomerCompletedSlots()
        {
            var customerId = UserService.GetUser();
            var customerSlotModelsResponse = await this.customerBookedSlotRepository.GetCustomerCompletedSlots(customerId);

            if (customerSlotModelsResponse.ResultType == ResultType.Success)
            {
                return await GetCustomerSlots(customerSlotModelsResponse);
            }

            return Response<IEnumerable<BookedSlotModel>>.Empty(new List<string>() { AppBusinessMessages.NoSlotsFound });
        }

        private async Task<Response<IEnumerable<BookedSlotModel>>> GetCustomerSlots(Response<IEnumerable<SlotModel>> customerSlotModelsResponse)
        {
            var customerIds = customerSlotModelsResponse.Result.Select(a => a.CreatedBy);
            var customerModelsResponse = await this.customerBusiness.GetCustomersByCustomerIds(customerIds);

            var BookedSlotModels = new List<BookedSlotModel>();
            foreach (var slotModel in customerSlotModelsResponse.Result)
            {
                var bookedSlotModel = new BookedSlotModel();
                bookedSlotModel.SlotModel = slotModel;
                bookedSlotModel.CreatedByCustomerModel = customerModelsResponse.Result.First(a => a.Id == slotModel.CreatedBy);

                BookedSlotModels.Add(bookedSlotModel);
            }

            return new Response<IEnumerable<BookedSlotModel>>() { Result = BookedSlotModels };
        }
    }
}
