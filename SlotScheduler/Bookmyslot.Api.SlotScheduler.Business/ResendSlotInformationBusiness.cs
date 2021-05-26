using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Email;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.Customers.Emails;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Business
{
    public class ResendSlotInformationBusiness : IResendSlotInformationBusiness
    {
        private readonly IEmailInteraction emailInteraction;
        private readonly ICustomerBusiness customerBusiness;
        public ResendSlotInformationBusiness(IEmailInteraction emailInteraction, ICustomerBusiness customerBusiness)
        {
            this.emailInteraction = emailInteraction;
            this.customerBusiness = customerBusiness;
        }

        public async Task<Response<bool>> ResendSlotMeetingInformation(SlotModel slotModel, string resendTo)
        {
            var customerModelsResponse = await this.customerBusiness.GetCustomerById(resendTo);
            var resendToCustomerModel = customerModelsResponse.Result;

            var emailModel = CustomerEmailTemplateFactory.SlotMeetingInformationTemplate(slotModel, resendToCustomerModel);
            return await this.emailInteraction.SendEmail(emailModel);
        }

    }
}
