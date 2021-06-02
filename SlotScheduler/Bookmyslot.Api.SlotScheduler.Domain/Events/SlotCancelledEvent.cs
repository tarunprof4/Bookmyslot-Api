using Bookmyslot.Api.Common.Contracts.Event;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookmyslot.Api.SlotScheduler.Domain.Events
{
   
    public class SlotCancelledEvent : BaseDomainEvent
    {
        public CancelledSlotModel CancelledSlotModel { get; set; }
        public string CancelledBy { get; set; }

        public SlotCancelledEvent(CancelledSlotModel cancelledSlotModel, string cancelledBy)
        {
            this.CancelledSlotModel = cancelledSlotModel;
            this.CancelledBy = cancelledBy;
        }
    }
}

