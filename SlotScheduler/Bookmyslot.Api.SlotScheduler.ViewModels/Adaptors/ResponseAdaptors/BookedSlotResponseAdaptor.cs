using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.Api.SlotScheduler.ViewModels.Adaptors.ResponseAdaptors.Interfaces;
using Bookmyslot.SharedKernel;
using Bookmyslot.SharedKernel.Contracts.Encryption;
using Bookmyslot.SharedKernel.ValueObject;
using Newtonsoft.Json;
using System;

namespace Bookmyslot.Api.SlotScheduler.ViewModels.Adaptors.ResponseAdaptors
{
    public class BookedSlotResponseAdaptor : IBookedSlotResponseAdaptor
    {
        private readonly ISymmetryEncryption symmetryEncryption;
        private readonly ICustomerResponseAdaptor customerResponseAdaptor;

        public BookedSlotResponseAdaptor(ISymmetryEncryption symmetryEncryption, ICustomerResponseAdaptor customerResponseAdaptor)
        {
            this.symmetryEncryption = symmetryEncryption;
            this.customerResponseAdaptor = customerResponseAdaptor;
        }


        public Result<BookedSlotViewModel> CreateBookedSlotViewModel(Result<BookedSlotModel> bookedSlotModelResponse)
        {
            if (bookedSlotModelResponse.ResultType == ResultType.Success)
            {
                var bookedSlotModel = bookedSlotModelResponse.Value;
                var bookedSlotViewModel = new BookedSlotViewModel
                {
                    BookedByCustomerCountry = bookedSlotModel.CustomerSettingsModel != null ? bookedSlotModel.CustomerSettingsModel.Country : string.Empty,
                };

                foreach (var bookedSlot in bookedSlotModel.BookedSlotModels)
                {
                    var slotInformation = this.symmetryEncryption.Encrypt(JsonConvert.SerializeObject(bookedSlot.Value.SlotModel));
                    var createdByCustomerViewModel = this.customerResponseAdaptor.CreateCustomerViewModel(bookedSlot.Key);

                    var slotModel = bookedSlot.Value.SlotModel;
                    var slotInformationInCustomerTimeZoneViewModel = new SlotInformationInCustomerTimeZoneViewModel()
                    {
                        Title = slotModel.Title,
                        Country = slotModel.Country,
                        SlotDuration = slotModel.SlotDuration,
                        SlotStartZonedDateTime = slotModel.SlotStartZonedDateTime,
                        SlotMeetingLink = slotModel.SlotMeetingLink,
                        CustomerSlotStartZonedDateTime = bookedSlot.Value.CustomerSlotZonedDateTime,
                        SlotInformation = slotInformation,
                    };
                    bookedSlotViewModel.BookedSlotModels.Add(new Tuple<CustomerViewModel, SlotInformationInCustomerTimeZoneViewModel>(createdByCustomerViewModel, slotInformationInCustomerTimeZoneViewModel));
                }

                return new Result<BookedSlotViewModel>() { Value = bookedSlotViewModel };
            }

            return new Result<BookedSlotViewModel>()
            {
                ResultType = bookedSlotModelResponse.ResultType,
                Messages = bookedSlotModelResponse.Messages
            };
        }
    }
}
