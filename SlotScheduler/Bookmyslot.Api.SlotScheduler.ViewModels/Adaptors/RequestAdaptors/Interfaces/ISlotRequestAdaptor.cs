﻿using Bookmyslot.Api.SlotScheduler.Domain;

namespace Bookmyslot.Api.SlotScheduler.ViewModels.Adaptors.RequestAdaptors.Interfaces
{
    public interface ISlotRequestAdaptor
    {
        SlotModel CreateSlotModel(SlotViewModel slotViewModel);
    }
}
