
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Encryption;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.ViewModels.Adaptors.ResponseAdaptors.Interfaces;
using Newtonsoft.Json;
using System;

namespace Bookmyslot.Api.SlotScheduler.ViewModels.Adaptors.ResponseAdaptors
{
    public class SharedSlotResponseAdaptor : ISharedSlotResponseAdaptor
    {
        private readonly ISymmetryEncryption symmetryEncryption;
        private readonly ICustomerResponseAdaptor customerResponseAdaptor;

        public SharedSlotResponseAdaptor(ISymmetryEncryption symmetryEncryption, ICustomerResponseAdaptor customerResponseAdaptor)
        {
            this.symmetryEncryption = symmetryEncryption;
            this.customerResponseAdaptor = customerResponseAdaptor;
        }
        public Response<SharedSlotViewModel> CreateSharedSlotViewModel(Response<SharedSlotModel> sharedSlotModelResponse)
        {
            if (sharedSlotModelResponse.ResultType == ResultType.Success)
            {
                var sharedSlotModel = sharedSlotModelResponse.Result;

                var sharedSlotViewModel = new SharedSlotViewModel();
                foreach (var sharedSlot in sharedSlotModel.SharedSlotModels)
                {
                    var slotInformation = this.symmetryEncryption.Encrypt(JsonConvert.SerializeObject(sharedSlot.Value));
                    var bookedByCustomerViewModel = sharedSlot.Key != null ? this.customerResponseAdaptor.CreateCustomerViewModel(sharedSlot.Key) : null;
                    sharedSlotViewModel.SharedSlotModels.Add(new Tuple<CustomerViewModel, SlotModel, string>(bookedByCustomerViewModel, sharedSlot.Value, slotInformation));
                }

                return new Response<SharedSlotViewModel>() { Result = sharedSlotViewModel };
            }

            return new Response<SharedSlotViewModel>()
            {
                ResultType = sharedSlotModelResponse.ResultType,
                Messages = sharedSlotModelResponse.Messages
            };
        }
    }
}
