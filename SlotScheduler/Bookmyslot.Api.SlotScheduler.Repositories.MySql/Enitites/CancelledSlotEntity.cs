using System;

namespace Bookmyslot.Api.SlotScheduler.Repositories.Enitites
{
    public class CancelledSlotEntity
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string CreatedBy { get; set; }
        public string CancelledBy { get; set; }
        public string BookedBy { get; set; }
        public string Country { get; set; }
        public string TimeZone { get; set; }
        public string SlotDate { get; set; }
        public DateTime SlotStartDateTimeUtc { get; set; }
        public DateTime SlotEndDateTimeUtc { get; set; }
        public TimeSpan SlotStartTime { get; set; }
        public TimeSpan SlotEndTime { get; set; }
        public DateTime CreatedDateUtc { get; set; }

    }
}
