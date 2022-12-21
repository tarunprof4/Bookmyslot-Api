using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.SharedKernel.Contracts.Event;
using Bookmyslot.SharedKernel.ValueObject;
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

        public async Task<Result<bool>> ResendSlotMeetingInformation(SlotModel slotModel, string resendTo)
        {
            var resendToCustomerModel = await this.customerBusiness.GetCustomerById(resendTo);
            slotModel.ResendSlotMeetingInformation(resendToCustomerModel.Value);
            await this.eventDispatcher.DispatchEvents(slotModel.Events);

            return new Result<bool>() { Value = true };
        }
    }

}
