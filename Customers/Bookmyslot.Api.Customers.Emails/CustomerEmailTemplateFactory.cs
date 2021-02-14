using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Emails.ViewModels;
using Bookmyslot.Api.SlotScheduler.Contracts;
using RazorEngine;
using RazorEngine.Templating;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Bookmyslot.Api.Customers.Emails
{
    public static class CustomerEmailTemplateFactory
    {
        

        public static EmailModel GetCustomerRegistrationWelcomeEmailTemplate(CustomerModel newCustomerModel)
        {
            if (!Engine.Razor.IsTemplateCached(TemplateConstants.CustomerRegistrationWelcomeEmailTemplateKey, typeof(CustomerModel)))
            {
                var customerRegistrationWelcomeEmailTemplateBody = GetTemplateBody(TemplateConstants.TemplateCustomerRegistationWelcomeEmail);
                var compiledMessageBody = Engine.Razor.RunCompile(customerRegistrationWelcomeEmailTemplateBody, TemplateConstants.CustomerRegistrationWelcomeEmailTemplateKey, typeof(CustomerModel), newCustomerModel);
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
                var slotScheduledTemplateBody = GetTemplateBody(TemplateConstants.TemplateSlotScheduledNotification);
                var compiledMessageBody = Engine.Razor.RunCompile(slotScheduledTemplateBody, TemplateConstants.SlotScheduledEmailTemplateKey, typeof(SlotSchedulerViewModel), slotSchedulerViewModel);
                return CreateEmailModel(bookedBy, TemplateConstants.SlotScheduledEmailSubject, compiledMessageBody);
            }

            var messageBody = Engine.Razor.Run(TemplateConstants.SlotScheduledEmailTemplateKey, typeof(SlotSchedulerViewModel), slotSchedulerViewModel);
            return CreateEmailModel(bookedBy, TemplateConstants.SlotScheduledEmailSubject, messageBody);
        }

        public static EmailModel SlotCancelledEmailTemplate(SlotModel slotModel, CustomerModel cancelledByCustomerModel)
        {
            var slotSchedulerViewModel = new SlotSchedulerViewModel() { SlotModel = slotModel, CustomerModel = cancelledByCustomerModel };
            if (!Engine.Razor.IsTemplateCached(TemplateConstants.SlotCancelledEmailTemplateKey, typeof(SlotSchedulerViewModel)))
            {
                var slotCancelledTemplateBody = GetTemplateBody(TemplateConstants.TemplateSlotCancelledNotification);
                var compiledMessageBody = Engine.Razor.RunCompile(slotCancelledTemplateBody, TemplateConstants.SlotCancelledEmailTemplateKey, typeof(SlotSchedulerViewModel), slotSchedulerViewModel);
                return CreateEmailModel(cancelledByCustomerModel, TemplateConstants.SlotCancelledEmailSubject, compiledMessageBody);
            }

            var messageBody = Engine.Razor.Run(TemplateConstants.SlotCancelledEmailTemplateKey, typeof(SlotSchedulerViewModel), slotSchedulerViewModel);
            return CreateEmailModel(cancelledByCustomerModel, TemplateConstants.SlotCancelledEmailSubject, messageBody);
        }

        public static EmailModel ResendSlotMeetingInformationTemplate(SlotModel slotModel, CustomerModel resendTo)
        {
            if (!Engine.Razor.IsTemplateCached(TemplateConstants.ResendSlotInformationEmailTemplateKey, typeof(CustomerModel)))
            {
                var resendSlotInformationTemplateBody = GetTemplateBody(TemplateConstants.TemplateResendSlotMeetingInformation);
                var compiledMessageBody = Engine.Razor.RunCompile(resendSlotInformationTemplateBody, TemplateConstants.ResendSlotInformationEmailTemplateKey, typeof(CustomerModel), resendTo);
                return CreateEmailModel(resendTo, TemplateConstants.ResendSlotInformationEmailSubject, compiledMessageBody);
            }

            var messageBody = Engine.Razor.Run(TemplateConstants.ResendSlotInformationEmailTemplateKey, typeof(CustomerModel), resendTo);
            return CreateEmailModel(resendTo, TemplateConstants.ResendSlotInformationEmailSubject, messageBody);
        }

      
        private static string GetTemplateBody(string templateName)
        {
            var directory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var templatesFolder = Path.Join(directory, TemplateConstants.Templates);
            var templatePath = Path.Join(templatesFolder, templateName);

            var content = string.Empty;
            using (var streamReader = new StreamReader(templatePath))
            {
                content = streamReader.ReadToEnd();
            }

            return content;
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
