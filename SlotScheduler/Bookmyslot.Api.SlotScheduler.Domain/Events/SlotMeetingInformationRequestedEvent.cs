using Bookmyslot.Api.Common.Contracts.Event;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookmyslot.Api.SlotScheduler.Domain.Events
{
   
    public class SlotMeetingInformationRequestedEvent : BaseDomainEvent
    {
        public SlotModel SlotModel { get; set; }
        public string ResendTo { get; set; }

        public SlotMeetingInformationRequestedEvent(SlotModel slotModel, string resendTo)
        {
            this.SlotModel = slotModel;
            this.ResendTo = resendTo;
        }
    }
}
