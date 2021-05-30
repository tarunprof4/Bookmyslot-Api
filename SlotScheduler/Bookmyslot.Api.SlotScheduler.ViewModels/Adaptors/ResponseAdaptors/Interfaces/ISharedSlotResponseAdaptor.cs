using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.SlotScheduler.Domain;

namespace Bookmyslot.Api.SlotScheduler.ViewModels.Adaptors.ResponseAdaptors.Interfaces
{
    public interface ISharedSlotResponseAdaptor
    {
        Response<SharedSlotViewModel> CreateSharedSlotViewModel(Response<SharedSlotModel> sharedSlotModelResponse);
    }
}
