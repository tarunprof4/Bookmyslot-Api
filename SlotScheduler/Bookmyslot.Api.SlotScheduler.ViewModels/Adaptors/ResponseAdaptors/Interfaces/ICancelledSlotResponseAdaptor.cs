using Bookmyslot.Api.SlotScheduler.Contracts;
using System.Collections.Generic;

namespace Bookmyslot.Api.SlotScheduler.ViewModels.Adaptors.ResponseAdaptors.Interfaces
{
    public interface ICancelledSlotResponseAdaptor
    {
        CancelledSlotViewModel CreateCancelledSlotViewModel(CancelledSlotModel cancelledSlotModel);
        IEnumerable<CancelledSlotViewModel> CreateCancelledSlotViewModels(IEnumerable<CancelledSlotModel> cancelledSlotModels);
        IEnumerable<CancelledSlotInformationViewModel> CreateCancelledSlotInformationViewModel(IEnumerable<CancelledSlotInformationModel> cancelledSlotInformationModels);
    }
}
