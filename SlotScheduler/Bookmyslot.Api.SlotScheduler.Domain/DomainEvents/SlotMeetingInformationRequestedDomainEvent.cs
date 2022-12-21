using Bookmyslot.Api.Customers.Domain;
using Bookmyslot.SharedKernel.Event;

namespace Bookmyslot.Api.SlotScheduler.Domain.DomainEvents
{

    public class SlotMeetingInformationRequestedDomainEvent : BaseDomainEvent
    {
        public SlotModel SlotModel { get; }
        public CustomerModel ResendToCustomerModel { get; }

        public SlotMeetingInformationRequestedDomainEvent(SlotModel slotModel, CustomerModel resendToCustomerModel)
        {
            this.SlotModel = slotModel;
            this.ResendToCustomerModel = resendToCustomerModel;
        }
    }
}
