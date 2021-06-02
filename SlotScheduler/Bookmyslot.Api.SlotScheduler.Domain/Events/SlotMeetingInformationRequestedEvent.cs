using Bookmyslot.Api.Common.Contracts.Event;

namespace Bookmyslot.Api.SlotScheduler.Domain.Events
{

    public class SlotMeetingInformationRequestedEvent : BaseDomainEvent
    {
        public string SlotId { get; set; }
        public string ResendTo { get; set; }

        public SlotMeetingInformationRequestedEvent(string slotId, string resendTo)
        {
            this.SlotId = slotId;
            this.ResendTo = resendTo;
        }
    }
}
