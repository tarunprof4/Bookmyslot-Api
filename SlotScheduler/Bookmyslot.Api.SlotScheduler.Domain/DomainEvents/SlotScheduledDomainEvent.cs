using Bookmyslot.Api.Common.Contracts.Event;
using Bookmyslot.Api.Customers.Domain;

namespace Bookmyslot.Api.SlotScheduler.Domain.DomainEvents
{
    public class SlotScheduledDomainEvent : BaseDomainEvent
    {
        public SlotModel SlotModel { get;  }
        public CustomerModel CreatedByCustomerModel { get; }
        public CustomerModel BookedByCustomerModel { get; }

        public SlotScheduledDomainEvent(SlotModel slotModel, CustomerModel createdByCustomerModel, CustomerModel bookedByCustomerModel)
        {
            this.SlotModel = slotModel;
            this.CreatedByCustomerModel = createdByCustomerModel;
            this.BookedByCustomerModel = bookedByCustomerModel;
        }
    }
}
