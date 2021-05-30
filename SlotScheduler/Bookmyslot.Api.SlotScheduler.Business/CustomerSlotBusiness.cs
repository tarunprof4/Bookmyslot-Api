using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Helpers;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.Customers.Domain;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.Domain;
using NodaTime;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Business
{
    public class CustomerSlotBusiness : ICustomerSlotBusiness
    {
        private readonly ICustomerSlotRepository customerSlotRepository;
        private readonly ICustomerBusiness customerBusiness;
        private readonly ICustomerSettingsRepository customerSettingsRepository;

        public CustomerSlotBusiness(ICustomerSlotRepository customerSlotRepository, ICustomerBusiness customerBusiness, ICustomerSettingsRepository customerSettingsRepository)
        {
            this.customerSlotRepository = customerSlotRepository;
            this.customerBusiness = customerBusiness;
            this.customerSettingsRepository = customerSettingsRepository;
        }


        public async Task<Response<List<CustomerSlotModel>>> GetDistinctCustomersNearestSlotFromToday(PageParameterModel pageParameterModel)
        {
            var allCustomerSlotsResponse = await this.customerSlotRepository.GetDistinctCustomersNearestSlotFromToday(pageParameterModel);
            if (allCustomerSlotsResponse.ResultType == ResultType.Success)
            {
                var customerSlotModels = new List<CustomerSlotModel>();
                var customerIds = allCustomerSlotsResponse.Result;

                var customerModels = await this.customerBusiness.GetCustomersByCustomerIds(customerIds);

                foreach (var customerModel in customerModels.Result)
                {
                    var customerSlotModel = new CustomerSlotModel
                    {
                        CustomerModel = customerModel
                    };
                    customerSlotModels.Add(customerSlotModel);
                }

                return Response<List<CustomerSlotModel>>.Success(customerSlotModels);
            }

            return Response<List<CustomerSlotModel>>.Empty(new List<string>() { AppBusinessMessagesConstants.NoRecordsFound }); ;
        }

        public async Task<Response<BookAvailableSlotModel>> GetCustomerAvailableSlots(PageParameterModel pageParameterModel, string customerId, string createdBy)
        {
            if (string.IsNullOrWhiteSpace(createdBy))
            {
                return Response<BookAvailableSlotModel>.ValidationError(new List<string>() { AppBusinessMessagesConstants.CustomerIdNotValid });
            }

            var allCustomerSlotsResponse = await this.customerSlotRepository.GetCustomerAvailableSlots(pageParameterModel, createdBy);
            if (allCustomerSlotsResponse.ResultType == ResultType.Success)
            {
                var allCustomerSlots = allCustomerSlotsResponse.Result;

                var customerModelResponse = this.customerBusiness.GetCustomerById(createdBy);
                var customerSettingsResponse = this.customerSettingsRepository.GetCustomerSettings(customerId);
                await Task.WhenAll(customerModelResponse, customerSettingsResponse);

                return CreateBookAvailableSlotModel(allCustomerSlots, customerModelResponse.Result, customerSettingsResponse.Result);
            }

            return Response<BookAvailableSlotModel>.Empty(new List<string>() { AppBusinessMessagesConstants.NoRecordsFound }); ;
        }

        private static Response<BookAvailableSlotModel> CreateBookAvailableSlotModel(IEnumerable<SlotModel> allCustomerSlots, Response<CustomerModel> customerModel, Response<CustomerSettingsModel> customerSettingsModel)
        {
            var bookAvailableSlotModel = new BookAvailableSlotModel
            {
                CreatedByCustomerModel = customerModel.Result,
                CustomerSettingsModel = customerSettingsModel.Result,
                AvailableSlotModels = new List<SlotInforamtionInCustomerTimeZoneModel>()
            };

            foreach (var allCustomerSlot in allCustomerSlots)
            {
                ZonedDateTime customerZonedDateTime = customerSettingsModel.ResultType != ResultType.Success ?
                    new ZonedDateTime() :
                    NodaTimeHelper.ConvertZonedDateTimeToZonedDateTime(allCustomerSlot.SlotStartZonedDateTime, bookAvailableSlotModel.CustomerSettingsModel.TimeZone);

                bookAvailableSlotModel.AvailableSlotModels.Add(new SlotInforamtionInCustomerTimeZoneModel() { SlotModel = allCustomerSlot, CustomerSlotZonedDateTime = customerZonedDateTime });
            }

            return Response<BookAvailableSlotModel>.Success(bookAvailableSlotModel);
        }


    }
}
