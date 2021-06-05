using Bookmyslot.Api.Common.Contracts.Event;

namespace Bookmyslot.Api.SlotScheduler.Domain.DomainEvents
{

    public class SlotCancelledDomainEvent : BaseDomainEvent
    {
        public CancelledSlotModel CancelledSlotModel { get; set; }
        public string CancelledBy { get; set; }

        public SlotCancelledDomainEvent(CancelledSlotModel cancelledSlotModel, string cancelledBy)
        {
            this.CancelledSlotModel = cancelledSlotModel;
            this.CancelledBy = cancelledBy;
        }
    }
}

