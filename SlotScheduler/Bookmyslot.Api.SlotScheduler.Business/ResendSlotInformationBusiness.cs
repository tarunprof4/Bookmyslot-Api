using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Event.Interfaces;
using Bookmyslot.Api.Customers.Domain;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.Domain;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Business
{
    public class ResendSlotInformationBusiness : IResendSlotInformationBusiness
    {
        private readonly IEventDispatcher eventDispatcher;
        public ResendSlotInformationBusiness(IEventDispatcher eventDispatcher)
        {
            this.eventDispatcher = eventDispatcher;
        }

        public async Task<Response<bool>> ResendSlotMeetingInformation(SlotModel slotModel, CustomerSummaryModel resendToCustomerSummaryModel)
        {
            slotModel.ResendSlotMeetingInformation(resendToCustomerSummaryModel);
            await this.eventDispatcher.DispatchEvents(slotModel.Events);

            return new Response<bool>() { Result = true };
        }
    }

}
