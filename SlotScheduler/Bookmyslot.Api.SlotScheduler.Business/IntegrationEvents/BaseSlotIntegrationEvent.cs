using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Contracts.Event;
using Bookmyslot.Api.Common.Helpers;
using Bookmyslot.Api.Customers.Domain.Constants;
using Bookmyslot.Api.SlotScheduler.Domain;
using System;

namespace Bookmyslot.Api.SlotScheduler.Business.IntegrationEvents
{
    public abstract class BaseSlotIntegrationEvent : IntegrationEvent
    {
        public string Title { get; }
        public string Country { get; }
        public string TimeZone { get;  }
        public string SlotDate { get;  }
        public TimeSpan SlotStartTime { get;  }
        public TimeSpan SlotEndTime { get;  }

        public TimeSpan SlotDuration { get; }

        public string SlotMeetingLink { get;  }

        public BaseSlotIntegrationEvent(SlotModel slotModel, string eventType):
            base(eventType)
        {
            this.Title = slotModel.Title;
            this.Country = slotModel.Country;
            this.TimeZone = slotModel.SlotStartZonedDateTime.Zone.Id;
            this.SlotDate = NodaTimeHelper.FormatLocalDate(slotModel.SlotStartZonedDateTime.Date, DateTimeConstants.ApplicationOutPutDatePattern);
            this.SlotStartTime = slotModel.SlotStartTime;
            this.SlotEndTime = slotModel.SlotEndTime;
            this.SlotDuration = slotModel.SlotDuration;
            this.SlotMeetingLink = slotModel.SlotMeetingLink;
        }
      

    }
}
