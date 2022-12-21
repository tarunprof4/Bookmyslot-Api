using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.SharedKernel.ValueObject;

namespace Bookmyslot.Api.SlotScheduler.ViewModels.Adaptors.ResponseAdaptors.Interfaces
{
    public interface IBookedSlotResponseAdaptor
    {
        Result<BookedSlotViewModel> CreateBookedSlotViewModel(Result<BookedSlotModel> bookedSlotModelResponse);
    }
}
