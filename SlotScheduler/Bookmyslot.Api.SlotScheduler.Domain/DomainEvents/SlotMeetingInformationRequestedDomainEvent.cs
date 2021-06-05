using Bookmyslot.Api.Common.Contracts.Event;
using Bookmyslot.Api.Customers.Domain;

namespace Bookmyslot.Api.SlotScheduler.Domain.DomainEvents
{

    public class SlotMeetingInformationRequestedDomainEvent : BaseDomainEvent
    {
        public SlotModel SlotModel { get; set; }
        public CustomerSummaryModel ResendToCustomerSummaryModel { get; }

        public SlotMeetingInformationRequestedDomainEvent(SlotModel slotModel, CustomerSummaryModel resendToCustomerSummaryModel)
        {
            this.SlotModel = slotModel;
            this.ResendToCustomerSummaryModel = resendToCustomerSummaryModel;
        }
    }
}
