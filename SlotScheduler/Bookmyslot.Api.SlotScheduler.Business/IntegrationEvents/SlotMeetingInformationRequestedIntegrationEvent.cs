using Bookmyslot.Api.Customers.Domain;
using Bookmyslot.Api.Customers.Domain.Constants;
using Bookmyslot.Api.SlotScheduler.Domain;

namespace Bookmyslot.Api.SlotScheduler.Business.IntegrationEvents
{

    public class SlotMeetingInformationRequestedIntegrationEvent : BaseSlotIntegrationEvent
    {
        public CustomerModel ResendToCustomerModel { get; }
        public SlotMeetingInformationRequestedIntegrationEvent(SlotModel slotModel, CustomerModel resendToCustomerModel)
            : base(slotModel, EventConstants.SlotMeetingInformationRequestedEvent)
        {
            this.ResendToCustomerModel = resendToCustomerModel;
        }
    }
}
