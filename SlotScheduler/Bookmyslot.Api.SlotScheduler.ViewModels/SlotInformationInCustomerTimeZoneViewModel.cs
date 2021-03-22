using NodaTime;
using System;

namespace Bookmyslot.Api.SlotScheduler.ViewModels
{
    public class SlotInformationInCustomerTimeZoneViewModel
    {
        public string Title { get; set; }

        public string Country { get; set; }

        public TimeSpan SlotDuration { get; set; }

        public ZonedDateTime SlotStartZonedDateTime { get; set; }

        public ZonedDateTime CustomerSlotStartZonedDateTime { get; set; }

        public string SlotMeetingLink { get; set; }

        public string SlotInformation { get; set; }
    }
}
