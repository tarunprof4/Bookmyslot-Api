using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Contracts.Event;
using Bookmyslot.Api.Common.Helpers;
using Bookmyslot.Api.SlotScheduler.Domain;

namespace Bookmyslot.Api.SlotScheduler.Business.IntegrationEvents
{
    public abstract class BaseSlotIntegrationEvent : IntegrationEvent
    {
        protected string Title { get; set; }
        protected string Country { get; set; }
        protected string TimeZone { get; set; }
        protected string SlotDate { get; set; }
        protected string StartTime { get; set; }
        protected string EndTime { get; set; }

        protected string Duration { get; set; }

        protected string MeetingLink { get; set; }


        protected void SetBaseSlotIntegrationEvent(SlotModel slotModel)
        {
            this.Title = slotModel.Title;
            this.Country = slotModel.Country;
            this.TimeZone = slotModel.SlotStartZonedDateTime.Zone.Id;
            this.SlotDate = NodaTimeHelper.FormatLocalDate(slotModel.SlotStartZonedDateTime.Date, DateTimeConstants.ApplicationOutPutDatePattern);
            this.StartTime = slotModel.SlotStartTime.ToString();
            this.EndTime = slotModel.SlotEndTime.ToString();
            this.Duration = slotModel.SlotDuration.TotalMinutes.ToString();
            this.MeetingLink = MeetingLink;
        }
    }
}
