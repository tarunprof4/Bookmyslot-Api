using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Helpers;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.ViewModels.Adaptors.RequestAdaptors.Interfaces;

namespace Bookmyslot.Api.SlotScheduler.ViewModels.Adaptors.RequestAdaptors
{
    public class SlotRequestAdaptor : ISlotRequestAdaptor
    {
        public SlotModel CreateSlotModel(SlotViewModel slotViewModel)
        {
            var slotModel = new SlotModel();
            slotModel.Title = slotViewModel.Title;
            slotModel.Country = slotViewModel.Country;
            var localDate = NodaTimeHelper.ConvertDateStringToLocalDateTime(slotViewModel.SlotDate, DateTimeConstants.ApplicationDatePattern, slotViewModel.SlotStartTime);
            slotModel.SlotStartZonedDateTime = NodaTimeHelper.ConvertLocalDateTimeToZonedDateTime(localDate, slotViewModel.TimeZone);
            slotModel.SlotStartTime = slotViewModel.SlotStartTime;
            slotModel.SlotEndTime = slotViewModel.SlotEndTime;
            return slotModel;
        }
    }
}
