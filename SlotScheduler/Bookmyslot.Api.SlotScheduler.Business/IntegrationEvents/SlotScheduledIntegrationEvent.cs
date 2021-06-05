using Bookmyslot.Api.SlotScheduler.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookmyslot.Api.SlotScheduler.Business.IntegrationEvents
{
    public class SlotScheduledIntegrationEvent : BaseSlotIntegrationEvent
    {
        public string BookedBy { get; }
        public SlotScheduledIntegrationEvent(SlotModel slotModel, string bookedBy)
        {
            base.SetBaseSlotIntegrationEvent(slotModel);
            this.BookedBy = bookedBy;
        }
    }
}
