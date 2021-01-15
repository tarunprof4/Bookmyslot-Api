﻿using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Emails.ViewModels;
using Bookmyslot.Api.SlotScheduler.Contracts;
using RazorEngine;
using RazorEngine.Templating;
using System.Collections.Generic;

namespace Bookmyslot.Api.Customers.Emails
{
    public static class CustomerEmailTemplateFactory
    {

        public static EmailModel GetCustomerRegistrationWelcomeEmailTemplate(CustomerModel newCustomerModel)
        {
            if (!Engine.Razor.IsTemplateCached(TemplateConstants.CustomerRegistrationWelcomeEmailTemplateKey, typeof(CustomerModel)))
            {
                var compiledMessageBody = Engine.Razor.RunCompile(TemplateBodyConstants.CustomerRegistrationWelcomeEmailTemplateBody, TemplateConstants.CustomerRegistrationWelcomeEmailTemplateKey, typeof(CustomerModel), newCustomerModel);
                return CreateEmailModel(newCustomerModel, TemplateConstants.CustomerRegistrationWelcomeEmailSubject, compiledMessageBody);
            }

            var messageBody = Engine.Razor.Run(TemplateConstants.CustomerRegistrationWelcomeEmailTemplateKey, typeof(CustomerModel), newCustomerModel);
            return CreateEmailModel(newCustomerModel, TemplateConstants.CustomerRegistrationWelcomeEmailSubject, messageBody);
        }

        public static EmailModel SlotScheduledEmailTemplate(SlotModel slotModel, CustomerModel bookedBy)
        {
            var slotSchedulerViewModel = new SlotSchedulerViewModel() { SlotModel = slotModel, CustomerModel = bookedBy };
            if (!Engine.Razor.IsTemplateCached(TemplateConstants.SlotScheduledEmailTemplateKey, typeof(SlotSchedulerViewModel)))
            {
                var compiledMessageBody = Engine.Razor.RunCompile(TemplateBodyConstants.SlotScheduledTemplateBody, TemplateConstants.SlotScheduledEmailTemplateKey, typeof(SlotSchedulerViewModel), slotSchedulerViewModel);
                return CreateEmailModel(bookedBy, TemplateConstants.SlotScheduledEmailSubject, compiledMessageBody);
            }

            var messageBody = Engine.Razor.Run(TemplateConstants.SlotScheduledEmailTemplateKey, typeof(SlotSchedulerViewModel), slotSchedulerViewModel);
            return CreateEmailModel(bookedBy, TemplateConstants.SlotScheduledEmailSubject, messageBody);
        }

        public static EmailModel GetResendSlotInformationEmailTemplate(SlotModel slotModel, CustomerModel resendTo)
        {
            if (!Engine.Razor.IsTemplateCached(TemplateConstants.ResendSlotInformationEmailTemplateKey, typeof(CustomerModel)))
            {
                var compiledMessageBody = Engine.Razor.RunCompile(TemplateBodyConstants.ResendSlotInformationTemplateBody, TemplateConstants.ResendSlotInformationEmailTemplateKey, typeof(CustomerModel), resendTo);
                return CreateEmailModel(resendTo, TemplateConstants.ResendSlotInformationEmailSubject, compiledMessageBody);
            }

            var messageBody = Engine.Razor.Run(TemplateConstants.ResendSlotInformationEmailTemplateKey, typeof(CustomerModel), resendTo);
            return CreateEmailModel(resendTo, TemplateConstants.ResendSlotInformationEmailSubject, messageBody);
        }

        public static EmailModel SlotCancelledEmailTemplate(CustomerModel cancelledByCustomerModel)
        {
            if (!Engine.Razor.IsTemplateCached(TemplateConstants.SlotCancelledEmailTemplateKey, typeof(CustomerModel)))
            {
                var compiledMessageBody = Engine.Razor.RunCompile(TemplateBodyConstants.SlotCancelledTemplateBody, TemplateConstants.SlotCancelledEmailTemplateKey, typeof(CustomerModel), cancelledByCustomerModel);
                return CreateEmailModel(cancelledByCustomerModel, TemplateConstants.SlotCancelledEmailSubject, compiledMessageBody);
            }

            var messageBody = Engine.Razor.Run(TemplateConstants.SlotCancelledEmailTemplateKey, typeof(CustomerModel), cancelledByCustomerModel);
            return CreateEmailModel(cancelledByCustomerModel, TemplateConstants.SlotCancelledEmailSubject, messageBody);
        }

        private static EmailModel CreateEmailModel(CustomerModel customerModel, string subject, string messageBody)
        {
            var emailModel = new EmailModel();
            emailModel.Subject = subject;
            emailModel.Body = messageBody;
            emailModel.To = new List<string>() { "tarun.aggarwal4@gmail.com" };
            emailModel.IsBodyHtml = true;
            return emailModel;
        }
    }
}
