using Bookmyslot.Api.Common.Contracts.Event;
using Bookmyslot.Api.Customers.Domain;

namespace Bookmyslot.Api.SlotScheduler.Domain.DomainEvents
{
    public class SlotScheduledDomainEvent : BaseDomainEvent
    {
        public SlotModel SlotModel { get; set; }
        public CustomerModel BookedByCustomerModel { get; set; }

        public SlotScheduledDomainEvent(SlotModel slotModel, CustomerModel bookedByCustomerModel)
        {
            this.SlotModel = slotModel;
            this.BookedByCustomerModel = bookedByCustomerModel;
        }
    }
}
