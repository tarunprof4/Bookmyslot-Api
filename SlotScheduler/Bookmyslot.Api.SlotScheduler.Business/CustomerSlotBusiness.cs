using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Business
{
    public class CustomerSlotBusiness : ICustomerSlotBusiness
    {
        private readonly ICustomerSlotRepository customerSlotRepository;
        private readonly ICustomerBusiness customerBusiness;
        public CustomerSlotBusiness(ICustomerSlotRepository customerSlotRepository, ICustomerBusiness customerBusiness)
        {
            this.customerSlotRepository = customerSlotRepository;
            this.customerBusiness = customerBusiness;
        }

        public async Task<Response<List<CustomerSlotModel>>> GetCustomerAvailableSlots(PageParameterModel pageParameterModel, string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return Response<List<CustomerSlotModel>>.ValidationError(new List<string>() { AppBusinessMessages.EmailIdMissing });
            }

            var allCustomerSlotsResponse = await this.customerSlotRepository.GetCustomerAvailableSlots(pageParameterModel, email);
            if (allCustomerSlotsResponse.ResultType == ResultType.Success)
            {
                var allCustomerSlots = allCustomerSlotsResponse.Result;

                var customerModelResponse = await this.customerBusiness.GetCustomer(email);
                var customerSlotModels = new List<CustomerSlotModel>();
                var customerSlotModel = new CustomerSlotModel
                {
                    SlotModels = allCustomerSlots.ToList(),
                    CustomerModel = customerModelResponse.Result
                };
                customerSlotModels.Add(customerSlotModel);

                return Response<List<CustomerSlotModel>>.Success(customerSlotModels);
            }

            return Response<List<CustomerSlotModel>>.Empty(new List<string>() { AppBusinessMessages.NoRecordsFound }); ;
        }

        public async Task<Response<List<CustomerSlotModel>>> GetDistinctCustomersNearestSlotFromToday(PageParameterModel pageParameterModel)
        {
            var allCustomerSlotsResponse = await this.customerSlotRepository.GetDistinctCustomersNearestSlotFromToday(pageParameterModel);
            if (allCustomerSlotsResponse.ResultType == ResultType.Success)
            {
                var customerSlotModels = new List<CustomerSlotModel>();

                var distinctCustomersLatestSlot = allCustomerSlotsResponse.Result;

                var responses = distinctCustomersLatestSlot.Select(async distinctCustomerLatestSlot =>
                {
                    var customerSlotModel = new CustomerSlotModel();
                    var customerModelResponse = await this.customerBusiness.GetCustomer(distinctCustomerLatestSlot.CreatedBy);
                    customerSlotModel.SlotModels = new List<SlotModel>() { distinctCustomerLatestSlot };
                    customerSlotModel.CustomerModel = customerModelResponse.Result;
                    customerSlotModels.Add(customerSlotModel);
                });

                await Task.WhenAll(responses);

                return Response<List<CustomerSlotModel>>.Success(customerSlotModels);
            }

            return Response<List<CustomerSlotModel>>.Empty(new List<string>() { AppBusinessMessages.NoRecordsFound }); ;
        }
    }
}
