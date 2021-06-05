using Bookmyslot.Api.Common.Contracts.Event;

namespace Bookmyslot.Api.SlotScheduler.Domain.DomainEvents
{

    public class SlotMeetingInformationRequestedDomainEvent : BaseDomainEvent
    {
        public string SlotId { get; set; }
        public string ResendTo { get; set; }

        public SlotMeetingInformationRequestedDomainEvent(string slotId, string resendTo)
        {
            this.SlotId = slotId;
            this.ResendTo = resendTo;
        }
    }
}
