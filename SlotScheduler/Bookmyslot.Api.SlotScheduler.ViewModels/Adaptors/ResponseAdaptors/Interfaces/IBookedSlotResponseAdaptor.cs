﻿using Bookmyslot.Api.Common.Contracts;

namespace Bookmyslot.Api.SlotScheduler.ViewModels.Adaptors.ResponseAdaptors.Interfaces
{
    public interface IBookedSlotResponseAdaptor
    {
        Response<BookedSlotViewModel> CreateBookedSlotViewModel(Response<BookedSlotModel> bookedSlotModelResponse);
    }
}
