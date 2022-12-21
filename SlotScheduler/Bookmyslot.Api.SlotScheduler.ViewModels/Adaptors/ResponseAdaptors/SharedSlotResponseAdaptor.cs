using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.Api.SlotScheduler.ViewModels.Adaptors.ResponseAdaptors.Interfaces;
using Bookmyslot.SharedKernel;
using Bookmyslot.SharedKernel.Contracts.Encryption;
using Bookmyslot.SharedKernel.ValueObject;
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
        public Result<SharedSlotViewModel> CreateSharedSlotViewModel(Result<SharedSlotModel> sharedSlotModelResponse)
        {
            if (sharedSlotModelResponse.ResultType == ResultType.Success)
            {
                var sharedSlotModel = sharedSlotModelResponse.Value;

                var sharedSlotViewModel = new SharedSlotViewModel();
                foreach (var sharedSlot in sharedSlotModel.SharedSlotModels)
                {
                    var slotInformation = this.symmetryEncryption.Encrypt(JsonConvert.SerializeObject(sharedSlot.Value));
                    var bookedByCustomerViewModel = sharedSlot.Key != null ? this.customerResponseAdaptor.CreateCustomerViewModel(sharedSlot.Key) : null;
                    sharedSlotViewModel.SharedSlotModels.Add(new Tuple<CustomerViewModel, SlotModel, string>(bookedByCustomerViewModel, sharedSlot.Value, slotInformation));
                }

                return new Result<SharedSlotViewModel>() { Value = sharedSlotViewModel };
            }

            return new Result<SharedSlotViewModel>()
            {
                ResultType = sharedSlotModelResponse.ResultType,
                Messages = sharedSlotModelResponse.Messages
            };
        }
    }
}
