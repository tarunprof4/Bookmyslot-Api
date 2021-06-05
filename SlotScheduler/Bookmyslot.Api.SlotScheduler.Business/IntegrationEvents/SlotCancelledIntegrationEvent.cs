using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Contracts.Event;
using Bookmyslot.Api.Common.Helpers;
using Bookmyslot.Api.SlotScheduler.Domain;

namespace Bookmyslot.Api.SlotScheduler.Business.IntegrationEvents
{
    public class SlotCancelledIntegrationEvent : IntegrationEvent
    {
        public string Title { get; set; }
        public string Country { get; set; }
        public string TimeZone { get; set; }
        public string SlotDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Duration { get; set; }
        public string CancelledBy { get; }
        public SlotCancelledIntegrationEvent(CancelledSlotModel cancelledSlotModel, string cancelledBy)
        {
            SetSlotCancelledIntegrationEvent(cancelledSlotModel);
            this.CancelledBy = cancelledBy;
        }

        private void SetSlotCancelledIntegrationEvent(CancelledSlotModel cancelledSlotModel)
        {
            this.Title = cancelledSlotModel.Title;
            this.Country = cancelledSlotModel.Country;
            this.TimeZone = cancelledSlotModel.SlotStartZonedDateTime.Zone.Id;
            this.SlotDate = NodaTimeHelper.FormatLocalDate(cancelledSlotModel.SlotStartZonedDateTime.Date, DateTimeConstants.ApplicationOutPutDatePattern);
            this.StartTime = cancelledSlotModel.SlotStartTime.ToString();
            this.EndTime = cancelledSlotModel.SlotEndTime.ToString();
            this.Duration = cancelledSlotModel.SlotDuration.TotalMinutes.ToString();
        }
    }
}
