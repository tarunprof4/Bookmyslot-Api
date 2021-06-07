using Bookmyslot.Api.Customers.Domain;
using Bookmyslot.Api.SlotScheduler.Domain;

namespace Bookmyslot.Api.SlotScheduler.Business.IntegrationEvents
{

    public class SlotMeetingInformationRequestedIntegrationEvent : BaseSlotIntegrationEvent
    {
        public CustomerModel ResendToCustomerModel { get; }
        public SlotMeetingInformationRequestedIntegrationEvent(SlotModel slotModel, CustomerModel resendToCustomerModel)
            : base(slotModel)
        {
            this.ResendToCustomerModel = resendToCustomerModel;
        }
    }
}
