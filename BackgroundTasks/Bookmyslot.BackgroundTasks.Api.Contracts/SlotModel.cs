using System;

namespace Bookmyslot.BackgroundTasks.Api.Contracts
{

    public class SlotModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string CreatedBy { get; set; }

        public string BookedBy { get; set; }

        public string Country { get; set; }

        //public ZonedDateTime SlotStartZonedDateTime { get; set; }

        public TimeSpan SlotStartTime { get; set; }

        public TimeSpan SlotEndTime { get; set; }

        public TimeSpan SlotDuration
        {
            get
            {
                return SlotEndTime - SlotStartTime;
            }
        }

        public string SlotMeetingLink { get; set; }

        public DateTime CreatedDateUtc { get; set; }




    }
}
