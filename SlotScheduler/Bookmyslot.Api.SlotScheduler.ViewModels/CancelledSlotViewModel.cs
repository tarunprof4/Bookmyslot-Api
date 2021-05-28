using NodaTime;
using System;

namespace Bookmyslot.Api.SlotScheduler.ViewModels
{
    public class CancelledSlotViewModel
    {
        public string Title { get; set; }

        public string Country { get; set; }
        public ZonedDateTime SlotStartZonedDateTime { get; set; }

        public TimeSpan SlotStartTime { get; set; }

        public TimeSpan SlotEndTime { get; set; }

        public TimeSpan SlotDuration
        {
            get
            {
                return SlotEndTime - SlotStartTime;
            }
        }
    }
}
