using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.SharedKernel.ValueObject;

namespace Bookmyslot.Api.SlotScheduler.ViewModels.Adaptors.ResponseAdaptors.Interfaces
{
    public interface ISharedSlotResponseAdaptor
    {
        Result<SharedSlotViewModel> CreateSharedSlotViewModel(Result<SharedSlotModel> sharedSlotModelResponse);
    }
}
