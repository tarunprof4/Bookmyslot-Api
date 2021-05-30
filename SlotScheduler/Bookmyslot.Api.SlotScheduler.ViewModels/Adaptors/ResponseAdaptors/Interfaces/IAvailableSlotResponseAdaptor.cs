
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.SlotScheduler.Domain;

namespace Bookmyslot.Api.SlotScheduler.ViewModels.Adaptors.ResponseAdaptors.Interfaces
{
    public interface IAvailableSlotResponseAdaptor
    {
        Response<BookAvailableSlotViewModel> CreateBookAvailableSlotViewModel(Response<BookAvailableSlotModel> bookAvailableSlotModelResponse);
    }
}
