
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Encryption;
using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.Api.SlotScheduler.ViewModels.Adaptors.ResponseAdaptors.Interfaces;
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


        public Response<BookedSlotViewModel> CreateBookedSlotViewModel(Response<BookedSlotModel> bookedSlotModelResponse)
        {
            if (bookedSlotModelResponse.ResultType == ResultType.Success)
            {
                var bookedSlotModel = bookedSlotModelResponse.Result;
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

                return new Response<BookedSlotViewModel>() { Result = bookedSlotViewModel };
            }

            return new Response<BookedSlotViewModel>()
            {
                ResultType = bookedSlotModelResponse.ResultType,
                Messages = bookedSlotModelResponse.Messages
            };
        }
    }
}
