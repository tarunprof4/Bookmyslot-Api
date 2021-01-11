using Bookmyslot.Api.Common;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Business
{
    public class ResendSlotInformationBusiness : IResendSlotInformationBusiness
    {
        private readonly IEmailInteraction emailInteraction;
        public ResendSlotInformationBusiness(IEmailInteraction emailInteraction)
        {
            this.emailInteraction = emailInteraction;
        }

        public async Task<Response<bool>> ResendSlotInformation(SlotModel slotModel, string resendTo)
        {
            var emailModel = new EmailModel();
            return await this.emailInteraction.SendEmail(emailModel);
        }
    }
}
