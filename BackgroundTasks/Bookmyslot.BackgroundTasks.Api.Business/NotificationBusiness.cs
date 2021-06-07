using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Email;
using Bookmyslot.BackgroundTasks.Api.Contracts;
using Bookmyslot.BackgroundTasks.Api.Contracts.Interfaces;
using Bookmyslot.BackgroundTasks.Api.Emails;
using System.Threading.Tasks;

namespace Bookmyslot.BackgroundTasks.Api.Business
{
    public class NotificationBusiness : INotificationBusiness
    {
        private readonly IEmailInteraction emailInteraction;
        public NotificationBusiness(IEmailInteraction emailInteraction)
        {
            this.emailInteraction = emailInteraction;
        }

        public async Task<Response<bool>> SendCustomerRegisteredNotification(CustomerModel customerModel)
        {
            var emailModel = CustomerEmailTemplateFactory.GetCustomerRegistrationWelcomeEmailTemplate(customerModel);
            return await this.emailInteraction.SendEmail(emailModel);
        }

        public async Task<Response<bool>> SlotCancelledNotificatiion(SlotModel slotModel, CustomerModel cancelledBy)
        {
            var emailModel = CustomerEmailTemplateFactory.SlotCancelledEmailTemplate(slotModel, cancelledBy);
            return await this.emailInteraction.SendEmail(emailModel);
        }

        public async Task<Response<bool>> SlotMeetingInformationNotification(SlotModel slotModel, CustomerModel resendTo)
        {
            var emailModel = CustomerEmailTemplateFactory.SlotMeetingInformationTemplate(slotModel, resendTo);
            return await this.emailInteraction.SendEmail(emailModel);
        }

        public async Task<Response<bool>> SlotScheduledNotificatiion(SlotModel slotModel, CustomerModel createdBy, CustomerModel bookedBy)
        {
            var createdByEmailModel = CustomerEmailTemplateFactory.SlotScheduledEmailTemplate(slotModel, createdBy);
            var bookedByEmailModel = CustomerEmailTemplateFactory.SlotScheduledEmailTemplate(slotModel, bookedBy);
            
            var sendEmailToCreatorTask =  this.emailInteraction.SendEmail(createdByEmailModel);
            var sendEmailToBookedByTask = this.emailInteraction.SendEmail(bookedByEmailModel);

            await Task.WhenAll(sendEmailToCreatorTask, sendEmailToBookedByTask);
            return sendEmailToBookedByTask.Result;
        }
    }
}
