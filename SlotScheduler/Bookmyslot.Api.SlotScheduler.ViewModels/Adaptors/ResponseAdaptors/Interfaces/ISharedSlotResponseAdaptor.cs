using Bookmyslot.Api.Common.Contracts;

namespace Bookmyslot.Api.SlotScheduler.ViewModels.Adaptors.ResponseAdaptors.Interfaces
{
    public interface ISharedSlotResponseAdaptor
    {
        Response<SharedSlotViewModel> CreateSharedSlotViewModel(Response<SharedSlotModel> sharedSlotModelResponse);
    }
}
