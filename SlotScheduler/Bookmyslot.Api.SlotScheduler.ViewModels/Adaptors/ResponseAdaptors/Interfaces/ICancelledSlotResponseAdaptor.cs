using System.Collections.Generic;

namespace Bookmyslot.Api.SlotScheduler.ViewModels.Adaptors.ResponseAdaptors.Interfaces
{
    public interface ICancelledSlotResponseAdaptor
    {
        IEnumerable<CancelledSlotViewModel> CreateCancelledSlotViewModels(IEnumerable<CancelledSlotModel> cancelledSlotModels);
        IEnumerable<CancelledSlotInformationViewModel> CreateCancelledSlotInformationViewModels(IEnumerable<CancelledSlotInformationModel> cancelledSlotInformationModels);
    }
}
