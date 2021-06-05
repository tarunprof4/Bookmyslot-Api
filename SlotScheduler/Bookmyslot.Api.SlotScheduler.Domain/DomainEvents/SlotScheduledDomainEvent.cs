using Bookmyslot.Api.Common.Contracts.Event;
using Bookmyslot.Api.Customers.Domain;

namespace Bookmyslot.Api.SlotScheduler.Domain.DomainEvents
{
    public class SlotScheduledDomainEvent : BaseDomainEvent
    {
        public SlotModel SlotModel { get; set; }
        public CustomerSummaryModel BookedByCustomerSummaryModel { get; }

        public SlotScheduledDomainEvent(SlotModel slotModel, CustomerSummaryModel bookedByCustomerSummaryModel)
        {
            this.SlotModel = slotModel;
            this.BookedByCustomerSummaryModel = bookedByCustomerSummaryModel;
        }
    }
}
