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

        public async Task<Response<List<CustomerSlotModel>>> GetCustomerSlots(PageParameterModel pageParameterModel, string email)
        {
            var allCustomerSlotsResponse = await this.customerSlotRepository.GetCustomerSlots(pageParameterModel, email);
            if (allCustomerSlotsResponse.ResultType == ResultType.Success)
            {
                var customerSlotModels = new List<CustomerSlotModel>();

                var allCustomerSlots = allCustomerSlotsResponse.Result;
                var distinctCustomers = allCustomerSlots.GroupBy(x => x.CreatedBy).Select(y => y.First().CreatedBy);

                var responses = distinctCustomers.Select(async customerEmail =>
                {
                    var customerSlotModel = new CustomerSlotModel();
                    var customerModelResponse = await this.customerBusiness.GetCustomer(customerEmail);
                    customerSlotModel.SlotModels = allCustomerSlots.Where(a => a.CreatedBy == customerEmail).ToList();
                    customerSlotModel.CustomerModel = customerModelResponse.Result;
                    customerSlotModels.Add(customerSlotModel);
                });

                await Task.WhenAll(responses);

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
