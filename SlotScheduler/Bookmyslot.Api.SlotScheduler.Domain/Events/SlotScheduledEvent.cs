using Bookmyslot.Api.Common.Contracts.Event;

namespace Bookmyslot.Api.SlotScheduler.Domain.Events
{
    public class SlotScheduledEvent : BaseDomainEvent
    {
        public string SlotId { get; set; }
        public string BookedBy { get; set; }

        public SlotScheduledEvent(string slotId, string bookedBy)
        {
            this.SlotId = slotId;
            this.BookedBy = bookedBy;
        }
    }
}
