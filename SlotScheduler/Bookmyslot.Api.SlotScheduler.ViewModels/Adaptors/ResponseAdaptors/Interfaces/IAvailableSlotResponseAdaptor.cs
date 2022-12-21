using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.SharedKernel.ValueObject;

namespace Bookmyslot.Api.SlotScheduler.ViewModels.Adaptors.ResponseAdaptors.Interfaces
{
    public interface IAvailableSlotResponseAdaptor
    {
        Result<BookAvailableSlotViewModel> CreateBookAvailableSlotViewModel(Result<BookAvailableSlotModel> bookAvailableSlotModelResponse);
    }
}
