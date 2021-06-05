using Bookmyslot.Api.SlotScheduler.Domain;

namespace Bookmyslot.Api.SlotScheduler.Business.IntegrationEvents
{

    public class SlotMeetingInformationRequestedIntegrationEvent : BaseSlotIntegrationEvent
    {
        public string ResendTo { get; }
        public SlotMeetingInformationRequestedIntegrationEvent(SlotModel slotModel, string resendTo)
            : base(slotModel)
        {
            this.ResendTo = resendTo;
        }
    }
}
