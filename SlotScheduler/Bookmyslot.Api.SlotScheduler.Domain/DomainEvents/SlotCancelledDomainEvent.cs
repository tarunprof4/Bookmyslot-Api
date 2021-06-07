using Bookmyslot.Api.Common.Contracts.Event;
using Bookmyslot.Api.Customers.Domain;

namespace Bookmyslot.Api.SlotScheduler.Domain.DomainEvents
{

    public class SlotCancelledDomainEvent : BaseDomainEvent
    {
        public CancelledSlotModel CancelledSlotModel { get;  }
        public CustomerModel CancelledByCustomerModel { get; }

        public SlotCancelledDomainEvent(CancelledSlotModel cancelledSlotModel, CustomerModel cancelledByCustomerModel)
        {
            this.CancelledSlotModel = cancelledSlotModel;
            this.CancelledByCustomerModel = cancelledByCustomerModel;
        }
    }
}

