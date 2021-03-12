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

       
        public async Task<Response<List<CustomerSlotModel>>> GetDistinctCustomersNearestSlotFromToday(PageParameterModel pageParameterModel)
        {



            var allCustomerSlotsResponse = await this.customerSlotRepository.GetDistinctCustomersNearestSlotFromToday(pageParameterModel);
            if (allCustomerSlotsResponse.ResultType == ResultType.Success)
            {
                var customerSlotModels = new List<CustomerSlotModel>();
                var customerIds = allCustomerSlotsResponse.Result;

                var customerModels = await this.customerBusiness.GetCustomersByCustomerIds(customerIds);

                foreach(var customerModel in customerModels.Result)
                {
                    var customerSlotModel = new CustomerSlotModel();
                    customerSlotModel.CustomerModel = customerModel;
                    customerSlotModels.Add(customerSlotModel);
                }

                return Response<List<CustomerSlotModel>>.Success(customerSlotModels);
            }

            return Response<List<CustomerSlotModel>>.Empty(new List<string>() { AppBusinessMessagesConstants.NoRecordsFound }); ;
        }

        public async Task<Response<BookSlotModel>> GetCustomerAvailableSlots(PageParameterModel pageParameterModel, string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                return Response<BookSlotModel>.ValidationError(new List<string>() { AppBusinessMessagesConstants.CustomerIdNotValid });
            }

            var allCustomerSlotsResponse = await this.customerSlotRepository.GetCustomerAvailableSlots(pageParameterModel, customerId);
            if (allCustomerSlotsResponse.ResultType == ResultType.Success)
            {
                var allCustomerSlots = allCustomerSlotsResponse.Result;

                var customerModelResponse = await this.customerBusiness.GetCustomerById(customerId);
                var bookSlotModel = new BookSlotModel
                {
                    CustomerModel = customerModelResponse.Result,
                    SlotModelsInforamtion = new List<BmsKeyValuePair<SlotModel, string>>()
                };

                foreach(var allCustomerSlot in allCustomerSlots)
                {
                    bookSlotModel.SlotModelsInforamtion.Add(new BmsKeyValuePair<SlotModel, string>(allCustomerSlot, string.Empty));
                }

                return Response<BookSlotModel>.Success(bookSlotModel);
            }

            return Response<BookSlotModel>.Empty(new List<string>() { AppBusinessMessagesConstants.NoRecordsFound }); ;
        }
    }
}
