using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Contracts.Event;
using Bookmyslot.Api.Common.Helpers;
using Bookmyslot.Api.SlotScheduler.Domain;

namespace Bookmyslot.Api.SlotScheduler.Business.IntegrationEvents
{
    public abstract class BaseSlotIntegrationEvent : IntegrationEvent
    {
        public string Title { get; }
        public string Country { get; }
        public string TimeZone { get;  }
        public string SlotDate { get;  }
        public string StartTime { get;  }
        public string EndTime { get;  }

        public string Duration { get; }

        public string MeetingLink { get;  }

        public BaseSlotIntegrationEvent(SlotModel slotModel)
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
