﻿using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Email;
using Bookmyslot.BackgroundTasks.Api.Contracts;
using Bookmyslot.BackgroundTasks.Api.Contracts.Interfaces.Business;
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

        public async Task<Response<bool>> SendCustomerRegisteredNotification(SearchCustomerModel customerModel)
        {
            var emailModel = CustomerEmailTemplateFactory.GetCustomerRegistrationWelcomeEmailTemplate(customerModel);
            return await this.emailInteraction.SendEmail(emailModel);
        }

        public async Task<Response<bool>> SlotCancelledNotificatiion(SlotModel slotModel, SearchCustomerModel cancelledBy, SearchCustomerModel notCancelledBy)
        {
            var emailModel = CustomerEmailTemplateFactory.SlotCancelledEmailTemplate(slotModel, cancelledBy, notCancelledBy);
            return await this.emailInteraction.SendEmail(emailModel);
        }

        public async Task<Response<bool>> SlotMeetingInformationNotification(SlotModel slotModel, SearchCustomerModel resendTo)
        {
            var emailModel = CustomerEmailTemplateFactory.SlotMeetingInformationTemplate(slotModel, resendTo);
            return await this.emailInteraction.SendEmail(emailModel);
        }

        public async Task<Response<bool>> SlotScheduledNotificatiion(SlotModel slotModel, SearchCustomerModel createdBy, SearchCustomerModel bookedBy)
        {
            var createdByEmailModel = CustomerEmailTemplateFactory.SlotScheduledEmailTemplate(slotModel, createdBy, bookedBy);

            return await this.emailInteraction.SendEmail(createdByEmailModel);


            //var createdByEmailModel = CustomerEmailTemplateFactory.SlotScheduledEmailTemplate(slotModel, createdBy, bookedBy);
            //var bookedByEmailModel = CustomerEmailTemplateFactory.SlotScheduledEmailTemplate(slotModel, bookedBy);

            //var sendEmailToCreatorTask =  this.emailInteraction.SendEmail(createdByEmailModel);
            //var sendEmailToBookedByTask = this.emailInteraction.SendEmail(bookedByEmailModel);

            //await Task.WhenAll(sendEmailToCreatorTask, sendEmailToBookedByTask);
            //return sendEmailToBookedByTask.Result;
        }
    }
}
