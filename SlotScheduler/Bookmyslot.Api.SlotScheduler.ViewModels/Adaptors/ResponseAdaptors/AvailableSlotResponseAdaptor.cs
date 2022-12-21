using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.Api.SlotScheduler.ViewModels.Adaptors.ResponseAdaptors.Interfaces;
using Bookmyslot.SharedKernel;
using Bookmyslot.SharedKernel.Contracts.Encryption;
using Bookmyslot.SharedKernel.ValueObject;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bookmyslot.Api.SlotScheduler.ViewModels.Adaptors.ResponseAdaptors
{
    public class AvailableSlotResponseAdaptor : IAvailableSlotResponseAdaptor
    {
        private readonly ISymmetryEncryption symmetryEncryption;
        private readonly ICustomerResponseAdaptor customerResponseAdaptor;

        public AvailableSlotResponseAdaptor(ISymmetryEncryption symmetryEncryption, ICustomerResponseAdaptor customerResponseAdaptor)
        {
            this.symmetryEncryption = symmetryEncryption;
            this.customerResponseAdaptor = customerResponseAdaptor;
        }


        public Result<BookAvailableSlotViewModel> CreateBookAvailableSlotViewModel(Result<BookAvailableSlotModel> bookAvailableSlotModelResponse)
        {
            if (bookAvailableSlotModelResponse.ResultType == ResultType.Success)
            {
                var bookAvailableSlotModel = bookAvailableSlotModelResponse.Value;
                var bookAvailableSlotViewModel = new BookAvailableSlotViewModel
                {
                    CreatedByCustomerViewModel = this.customerResponseAdaptor.CreateCustomerViewModel(bookAvailableSlotModel.CreatedByCustomerModel),
                    ToBeBookedByCustomerCountry = bookAvailableSlotModel.CustomerSettingsModel != null ? bookAvailableSlotModel.CustomerSettingsModel.Country : string.Empty,
                    BookAvailableSlotModels = new List<SlotInformationInCustomerTimeZoneViewModel>()
                };

                foreach (var availableSlotModel in bookAvailableSlotModel.AvailableSlotModels)
                {
                    var slotInformation = this.symmetryEncryption.Encrypt(JsonConvert.SerializeObject(availableSlotModel.SlotModel));
                    bookAvailableSlotViewModel.BookAvailableSlotModels.Add(new SlotInformationInCustomerTimeZoneViewModel()
                    {
                        Title = availableSlotModel.SlotModel.Title,
                        Country = availableSlotModel.SlotModel.Country,
                        SlotDuration = availableSlotModel.SlotModel.SlotDuration,
                        SlotStartZonedDateTime = availableSlotModel.SlotModel.SlotStartZonedDateTime,
                        CustomerSlotStartZonedDateTime = availableSlotModel.CustomerSlotZonedDateTime,
                        SlotInformation = slotInformation
                    });
                }

                return new Result<BookAvailableSlotViewModel>() { Value = bookAvailableSlotViewModel };
            }

            return new Result<BookAvailableSlotViewModel>()
            {
                ResultType = bookAvailableSlotModelResponse.ResultType,
                Messages = bookAvailableSlotModelResponse.Messages
            };
        }
    }
}
