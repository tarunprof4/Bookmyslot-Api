using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.BackgroundTasks.Api.Contracts;
using Bookmyslot.BackgroundTasks.Api.Emails.ViewModels;
using RazorEngine;
using RazorEngine.Templating;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Bookmyslot.BackgroundTasks.Api.Emails
{
    public static class CustomerEmailTemplateFactory
    {
        public static EmailModel GetCustomerRegistrationWelcomeEmailTemplate(CustomerModel newCustomerModel)
        {
            var registerCustomerViewModel = new RegisterCustomerViewModel(newCustomerModel);
            if (!Engine.Razor.IsTemplateCached(TemplateConstants.CustomerRegistrationWelcomeEmailTemplateKey, typeof(RegisterCustomerViewModel)))
            {
                var customerRegistrationWelcomeEmailTemplateBody = GetTemplateBody(TemplateConstants.TemplateCustomerRegistationWelcomeEmailNotification);
                var compiledMessageBody = Engine.Razor.RunCompile(customerRegistrationWelcomeEmailTemplateBody, TemplateConstants.CustomerRegistrationWelcomeEmailTemplateKey, typeof(RegisterCustomerViewModel), registerCustomerViewModel);
                return CreateEmailModel(newCustomerModel, TemplateConstants.CustomerRegistrationWelcomeEmailSubject, compiledMessageBody);
            }

            var messageBody = Engine.Razor.Run(TemplateConstants.CustomerRegistrationWelcomeEmailTemplateKey, typeof(RegisterCustomerViewModel), registerCustomerViewModel);
            return CreateEmailModel(newCustomerModel, TemplateConstants.CustomerRegistrationWelcomeEmailSubject, messageBody);
        }

        public static EmailModel SlotScheduledEmailTemplate(SlotModel slotModel, CustomerModel createdByCustomerModel, CustomerModel bookedByCustomerModel)
        {
            SlotSchedulerBookedViewModel slotSchedulerBookedViewModel = new SlotSchedulerBookedViewModel(slotModel, createdByCustomerModel, bookedByCustomerModel);

            if (!Engine.Razor.IsTemplateCached(TemplateConstants.SlotScheduledEmailTemplateKey, typeof(SlotSchedulerBookedViewModel)))
            {
                var slotScheduledTemplateBody = GetTemplateBody(TemplateConstants.TemplateSlotScheduledNotification);
                var compiledMessageBody = Engine.Razor.RunCompile(slotScheduledTemplateBody, TemplateConstants.SlotScheduledEmailTemplateKey, typeof(SlotSchedulerBookedViewModel), slotSchedulerBookedViewModel);
                return CreateEmailModel(createdByCustomerModel, TemplateConstants.SlotScheduledEmailSubject, compiledMessageBody);
            }

            var messageBody = Engine.Razor.Run(TemplateConstants.SlotScheduledEmailTemplateKey, typeof(SlotSchedulerBookedViewModel), slotSchedulerBookedViewModel);
            return CreateEmailModel(createdByCustomerModel, TemplateConstants.SlotScheduledEmailSubject, messageBody);
        }

        public static EmailModel SlotCancelledEmailTemplate(SlotModel slotModel, CustomerModel cancelledByCustomerModel, CustomerModel notCancelledByCustomerModel)
        {
            SlotSchedulerCancelledViewModel slotSchedulerCancelledByViewModel = new SlotSchedulerCancelledViewModel(slotModel,
                cancelledByCustomerModel, notCancelledByCustomerModel);
            
            if (!Engine.Razor.IsTemplateCached(TemplateConstants.SlotCancelledEmailTemplateKey, typeof(SlotSchedulerBookedViewModel)))
            {
                var slotCancelledTemplateBody = GetTemplateBody(TemplateConstants.TemplateSlotCancelledNotification);
                var compiledMessageBody = Engine.Razor.RunCompile(slotCancelledTemplateBody, TemplateConstants.SlotCancelledEmailTemplateKey, typeof(SlotSchedulerCancelledViewModel), slotSchedulerCancelledByViewModel);
                return CreateEmailModel(notCancelledByCustomerModel, TemplateConstants.SlotCancelledEmailSubject, compiledMessageBody);
            }

            var messageBody = Engine.Razor.Run(TemplateConstants.SlotCancelledEmailTemplateKey, typeof(SlotSchedulerCancelledViewModel), slotSchedulerCancelledByViewModel);
            return CreateEmailModel(notCancelledByCustomerModel, TemplateConstants.SlotCancelledEmailSubject, messageBody);
        }

        public static EmailModel SlotMeetingInformationTemplate(SlotModel slotModel, CustomerModel resendTo)
        {
            SlotMeetingViewModel slotMeetingViewModel = new SlotMeetingViewModel(slotModel);

            if (!Engine.Razor.IsTemplateCached(TemplateConstants.SlotMeetingInformationEmailTemplateKey, typeof(CustomerModel)))
            {
                var slotInformationTemplateBody = GetTemplateBody(TemplateConstants.TemplateSlotMeetingInformationNotification);
                var compiledMessageBody = Engine.Razor.RunCompile(slotInformationTemplateBody, TemplateConstants.SlotMeetingInformationEmailTemplateKey, typeof(SlotMeetingViewModel), slotMeetingViewModel);
                return CreateEmailModel(resendTo, TemplateConstants.SlotMeetingInformationEmailSubject, compiledMessageBody);
            }

            var messageBody = Engine.Razor.Run(TemplateConstants.SlotMeetingInformationEmailTemplateKey, typeof(SlotMeetingViewModel), slotMeetingViewModel);
            return CreateEmailModel(resendTo, TemplateConstants.SlotMeetingInformationEmailSubject, messageBody);
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



        //private static string GetTemplateBody(string templateName)
        //{
        //    if (templateName == TemplateConstants.TemplateCustomerRegistationWelcomeEmailNotification)
        //        return TemplateBodyConstants.CustomerRegistrationWelcomeEmailTemplateBody;

        //    else if (templateName == TemplateConstants.TemplateSlotScheduledNotification)
        //    {
        //        return TemplateBodyConstants.SlotScheduledTemplateBody;
        //    }

        //    else if (templateName == TemplateConstants.TemplateSlotCancelledNotification)
        //    {
        //        return TemplateBodyConstants.SlotCancelledTemplateBody;
        //    }

        //    else if (templateName == TemplateConstants.TemplateSlotMeetingInformationNotification)
        //    {
        //        return TemplateBodyConstants.SlotMeetingInformationTemplateBody;
        //    }


        //    return "";
        //}



        private static EmailModel CreateEmailModel(CustomerModel customerModel, string subject, string messageBody)
        {
            var emailModel = new EmailModel();
            emailModel.Subject = subject;
            emailModel.Body = messageBody;
            emailModel.To = new List<string>() { customerModel.Email };
            emailModel.IsBodyHtml = true;
            return emailModel;
        }




    
    }
}
