using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.Customers.Domain;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.SharedKernel;
using Bookmyslot.SharedKernel.Helpers;
using Bookmyslot.SharedKernel.ValueObject;
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


        public async Task<Result<List<CustomerSlotModel>>> GetDistinctCustomersNearestSlotFromToday(PageParameterModel pageParameterModel)
        {
            var allCustomerSlotsResponse = await this.customerSlotRepository.GetDistinctCustomersNearestSlotFromToday(pageParameterModel);
            if (allCustomerSlotsResponse.ResultType == ResultType.Success)
            {
                var customerSlotModels = new List<CustomerSlotModel>();
                var customerIds = allCustomerSlotsResponse.Value;

                var customerModels = await this.customerBusiness.GetCustomersByCustomerIds(customerIds);

                foreach (var customerModel in customerModels.Value)
                {
                    var customerSlotModel = new CustomerSlotModel
                    {
                        CustomerModel = customerModel
                    };
                    customerSlotModels.Add(customerSlotModel);
                }

                return Result<List<CustomerSlotModel>>.Success(customerSlotModels);
            }

            return Result<List<CustomerSlotModel>>.Empty(new List<string>() { AppBusinessMessagesConstants.NoRecordsFound }); ;
        }

        public async Task<Result<BookAvailableSlotModel>> GetCustomerAvailableSlots(PageParameterModel pageParameterModel, string customerId, string createdBy)
        {
            if (string.IsNullOrWhiteSpace(createdBy))
            {
                return Result<BookAvailableSlotModel>.ValidationError(new List<string>() { AppBusinessMessagesConstants.CustomerIdNotValid });
            }

            var allCustomerSlotsResponse = await this.customerSlotRepository.GetCustomerAvailableSlots(pageParameterModel, createdBy);
            if (allCustomerSlotsResponse.ResultType == ResultType.Success)
            {
                var allCustomerSlots = allCustomerSlotsResponse.Value;

                var customerModelResponse = this.customerBusiness.GetCustomerById(createdBy);
                var customerSettingsResponse = this.customerSettingsRepository.GetCustomerSettings(customerId);
                await Task.WhenAll(customerModelResponse, customerSettingsResponse);

                return CreateBookAvailableSlotModel(allCustomerSlots, customerModelResponse.Result, customerSettingsResponse.Result);
            }

            return Result<BookAvailableSlotModel>.Empty(new List<string>() { AppBusinessMessagesConstants.NoRecordsFound }); ;
        }

        private static Result<BookAvailableSlotModel> CreateBookAvailableSlotModel(IEnumerable<SlotModel> allCustomerSlots, Result<CustomerModel> customerModel, Result<CustomerSettingsModel> customerSettingsModel)
        {
            var bookAvailableSlotModel = new BookAvailableSlotModel
            {
                CreatedByCustomerModel = customerModel.Value,
                CustomerSettingsModel = customerSettingsModel.Value,
                AvailableSlotModels = new List<SlotInforamtionInCustomerTimeZoneModel>()
            };

            foreach (var allCustomerSlot in allCustomerSlots)
            {
                ZonedDateTime customerZonedDateTime = customerSettingsModel.ResultType != ResultType.Success ?
                    new ZonedDateTime() :
                    NodaTimeHelper.ConvertZonedDateTimeToZonedDateTime(allCustomerSlot.SlotStartZonedDateTime, bookAvailableSlotModel.CustomerSettingsModel.TimeZone);

                bookAvailableSlotModel.AvailableSlotModels.Add(new SlotInforamtionInCustomerTimeZoneModel() { SlotModel = allCustomerSlot, CustomerSlotZonedDateTime = customerZonedDateTime });
            }

            return Result<BookAvailableSlotModel>.Success(bookAvailableSlotModel);
        }


    }
}
