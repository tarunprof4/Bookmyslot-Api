using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Event.Interfaces;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.Domain;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Business
{
    public class ResendSlotInformationBusiness : IResendSlotInformationBusiness
    {
        private readonly IEventDispatcher eventDispatcher;
        private readonly ICustomerBusiness customerBusiness;
        public ResendSlotInformationBusiness(IEventDispatcher eventDispatcher, ICustomerBusiness customerBusiness)
        {
            this.eventDispatcher = eventDispatcher;
            this.customerBusiness = customerBusiness;
        }

        public async Task<Response<bool>> ResendSlotMeetingInformation(SlotModel slotModel, string resendTo)
        {
            var resendToCustomerModel = await this.customerBusiness.GetCustomerById(resendTo);
            slotModel.ResendSlotMeetingInformation(resendToCustomerModel.Result);
            await this.eventDispatcher.DispatchEvents(slotModel.Events);
            
            return new Response<bool>() { Result = true };
        }
    }

}
