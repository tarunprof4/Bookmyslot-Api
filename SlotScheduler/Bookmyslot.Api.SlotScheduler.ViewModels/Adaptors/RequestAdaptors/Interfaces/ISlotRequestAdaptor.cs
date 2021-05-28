using Bookmyslot.Api.SlotScheduler.Contracts;

namespace Bookmyslot.Api.SlotScheduler.ViewModels.Adaptors.RequestAdaptors.Interfaces
{
    public interface ISlotRequestAdaptor
    {
        SlotModel CreateSlotModel(SlotViewModel slotViewModel);
    }
}
