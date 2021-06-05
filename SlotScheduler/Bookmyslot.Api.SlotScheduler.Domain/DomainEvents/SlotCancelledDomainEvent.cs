using Bookmyslot.Api.Common.Contracts.Event;
using Bookmyslot.Api.Customers.Domain;

namespace Bookmyslot.Api.SlotScheduler.Domain.DomainEvents
{

    public class SlotCancelledDomainEvent : BaseDomainEvent
    {
        public CancelledSlotModel CancelledSlotModel { get;  }
        public CustomerSummaryModel CancelledByCustomerSummaryModel { get;  }

        public SlotCancelledDomainEvent(CancelledSlotModel cancelledSlotModel, CustomerSummaryModel cancelledByCustomerSummaryModel)
        {
            this.CancelledSlotModel = cancelledSlotModel;
            this.CancelledByCustomerSummaryModel = cancelledByCustomerSummaryModel;
        }
    }
}

