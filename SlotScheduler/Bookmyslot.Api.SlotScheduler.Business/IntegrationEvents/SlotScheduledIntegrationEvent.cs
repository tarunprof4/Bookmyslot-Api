using Bookmyslot.Api.SlotScheduler.Domain;

namespace Bookmyslot.Api.SlotScheduler.Business.IntegrationEvents
{
    public class SlotScheduledIntegrationEvent : BaseSlotIntegrationEvent
    {
        public string BookedBy { get; }
        public SlotScheduledIntegrationEvent(SlotModel slotModel, string bookedBy)
            :base(slotModel)
        {
            this.BookedBy = bookedBy;
        }
    }
}
