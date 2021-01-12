using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Interfaces;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.Customers.Emails;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using System.Collections.Generic;
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

            var resendEmailMessage = CustomerEmailTemplateFactory.GetResendSlotInformationTemplate(slotModel, resendToCustomerModel);

            var emailModel = CreateEmailModel(resendToCustomerModel, resendEmailMessage);
            return await this.emailInteraction.SendEmail(emailModel);
        }

        private static EmailModel CreateEmailModel(CustomerModel resendToCustomerModel, string resendEmailMessage)
        {
            var emailModel = new EmailModel();
            emailModel.Subject = TemplateConstants.ResendSlotInformationTemplateSubject;
            emailModel.Body = resendEmailMessage;
            emailModel.To = new List<string>() { resendToCustomerModel.Email };
            emailModel.IsBodyHtml = true;
            return emailModel;
        }
    }
}
