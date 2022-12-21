using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.Customers.Domain;
using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.SharedKernel;
using Bookmyslot.SharedKernel.Helpers;
using Bookmyslot.SharedKernel.ValueObject;
using NodaTime;
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
        private readonly ICustomerSettingsRepository customerSettingsRepository;
        public CustomerBookedSlotBusiness(ICustomerBookedSlotRepository customerBookedSlotRepository, ICustomerCancelledSlotRepository customerCancelledSlotRepository,
            ICustomerBusiness customerBusiness, ICustomerSettingsRepository customerSettingsRepository)
        {
            this.customerBookedSlotRepository = customerBookedSlotRepository;
            this.customerCancelledSlotRepository = customerCancelledSlotRepository;
            this.customerBusiness = customerBusiness;
            this.customerSettingsRepository = customerSettingsRepository;
        }

        public async Task<Result<IEnumerable<CancelledSlotInformationModel>>> GetCustomerCancelledSlots(string customerId)
        {
            var cancelledSlotsResponse = await this.customerCancelledSlotRepository.GetCustomerBookedCancelledSlots(customerId);

            if (cancelledSlotsResponse.ResultType == ResultType.Success)
            {
                var customerIds = cancelledSlotsResponse.Value.Select(a => a.CancelledBy);
                var customerModelsResponse = await this.customerBusiness.GetCustomersByCustomerIds(customerIds);

                var cancelledSlotInformationModels = new List<CancelledSlotInformationModel>();
                foreach (var cancelledSlotResponse in cancelledSlotsResponse.Value)
                {
                    var cancelledSlotInformationModel = new CancelledSlotInformationModel();
                    cancelledSlotInformationModel.CancelledSlotModel = cancelledSlotResponse;
                    cancelledSlotInformationModel.CancelledByCustomerModel = customerModelsResponse.Value.First(a => a.Id == cancelledSlotResponse.CancelledBy);

                    cancelledSlotInformationModels.Add(cancelledSlotInformationModel);
                }

                return new Result<IEnumerable<CancelledSlotInformationModel>>() { Value = cancelledSlotInformationModels };
            }

            return Result<IEnumerable<CancelledSlotInformationModel>>.Empty(new List<string>() { AppBusinessMessagesConstants.NoSlotsFound });
        }

        public async Task<Result<BookedSlotModel>> GetCustomerBookedSlots(string customerId)
        {
            var customerSlotModelsResponse = await this.customerBookedSlotRepository.GetCustomerBookedSlots(customerId);

            if (customerSlotModelsResponse.ResultType == ResultType.Success)
            {
                return await GetCustomerSlots(customerSlotModelsResponse, customerId);
            }

            return Result<BookedSlotModel>.Empty(new List<string>() { AppBusinessMessagesConstants.NoSlotsFound });
        }

        public async Task<Result<BookedSlotModel>> GetCustomerCompletedSlots(string customerId)
        {
            var customerSlotModelsResponse = await this.customerBookedSlotRepository.GetCustomerCompletedSlots(customerId);

            if (customerSlotModelsResponse.ResultType == ResultType.Success)
            {
                return await GetCustomerSlots(customerSlotModelsResponse, customerId);
            }

            return Result<BookedSlotModel>.Empty(new List<string>() { AppBusinessMessagesConstants.NoSlotsFound });
        }

        private async Task<Result<BookedSlotModel>> GetCustomerSlots(Result<IEnumerable<SlotModel>> customerSlotModelsResponse, string customerId)
        {
            var customerIds = customerSlotModelsResponse.Value.Select(a => a.CreatedBy);
            var customerModelsResponseTask = this.customerBusiness.GetCustomersByCustomerIds(customerIds);
            var customerSettingsResponseTask = this.customerSettingsRepository.GetCustomerSettings(customerId);
            await Task.WhenAll(customerModelsResponseTask, customerSettingsResponseTask);
            var customerModelsResponse = customerModelsResponseTask.Result;
            var customerSettingsResponse = customerSettingsResponseTask.Result;

            var bookedSlotModel = new BookedSlotModel();
            bookedSlotModel.BookedSlotModels = new List<KeyValuePair<CustomerModel, SlotInforamtionInCustomerTimeZoneModel>>();
            bookedSlotModel.CustomerSettingsModel = customerSettingsResponse.Value;
            foreach (var slotModel in customerSlotModelsResponse.Value)
            {
                var createdByCustomerModel = customerModelsResponse.Value.First(a => a.Id == slotModel.CreatedBy);
                ZonedDateTime customerZonedDateTime = customerSettingsResponse.ResultType != ResultType.Success ?
                   new ZonedDateTime() :
                   NodaTimeHelper.ConvertZonedDateTimeToZonedDateTime(slotModel.SlotStartZonedDateTime, customerSettingsResponse.Value.TimeZone);

                var slotInforamtionInCustomerTimeZoneModel = new SlotInforamtionInCustomerTimeZoneModel()
                { SlotModel = slotModel, CustomerSlotZonedDateTime = customerZonedDateTime };

                bookedSlotModel.BookedSlotModels.Add(new KeyValuePair<CustomerModel, SlotInforamtionInCustomerTimeZoneModel>(createdByCustomerModel, slotInforamtionInCustomerTimeZoneModel));
            }

            return new Result<BookedSlotModel>() { Value = bookedSlotModel };
        }
    }
}
