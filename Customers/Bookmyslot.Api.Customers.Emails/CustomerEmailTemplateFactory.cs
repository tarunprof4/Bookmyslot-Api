using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Helpers;
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
            var registerCustomerViewModel = CreateRegisterCustomerViewModel(newCustomerModel);
            if (!Engine.Razor.IsTemplateCached(TemplateConstants.CustomerRegistrationWelcomeEmailTemplateKey, typeof(RegisterCustomerViewModel)))
            {
                var customerRegistrationWelcomeEmailTemplateBody = GetTemplateBody(TemplateConstants.TemplateCustomerRegistationWelcomeEmail);
                var compiledMessageBody = Engine.Razor.RunCompile(customerRegistrationWelcomeEmailTemplateBody, TemplateConstants.CustomerRegistrationWelcomeEmailTemplateKey, typeof(RegisterCustomerViewModel), registerCustomerViewModel);
                return CreateEmailModel(newCustomerModel, TemplateConstants.CustomerRegistrationWelcomeEmailSubject, compiledMessageBody);
            }

            var messageBody = Engine.Razor.Run(TemplateConstants.CustomerRegistrationWelcomeEmailTemplateKey, typeof(RegisterCustomerViewModel), registerCustomerViewModel);
            return CreateEmailModel(newCustomerModel, TemplateConstants.CustomerRegistrationWelcomeEmailSubject, messageBody);
        }

        public static EmailModel SlotScheduledEmailTemplate(SlotModel slotModel, CustomerModel bookedBy)
        {
            var slotSchedulerBookedByViewModel =  CreateSlotSchedulerBookedViewModel(slotModel, bookedBy);
            if (!Engine.Razor.IsTemplateCached(TemplateConstants.SlotScheduledEmailTemplateKey, typeof(SlotSchedulerBookedViewModel)))
            {
                var slotScheduledTemplateBody = GetTemplateBody(TemplateConstants.TemplateSlotScheduledNotification);
                var compiledMessageBody = Engine.Razor.RunCompile(slotScheduledTemplateBody, TemplateConstants.SlotScheduledEmailTemplateKey, typeof(SlotSchedulerBookedViewModel), slotSchedulerBookedByViewModel);
                return CreateEmailModel(bookedBy, TemplateConstants.SlotScheduledEmailSubject, compiledMessageBody);
            }

            var messageBody = Engine.Razor.Run(TemplateConstants.SlotScheduledEmailTemplateKey, typeof(SlotSchedulerBookedViewModel), slotSchedulerBookedByViewModel);
            return CreateEmailModel(bookedBy, TemplateConstants.SlotScheduledEmailSubject, messageBody);
        }

        public static EmailModel SlotCancelledEmailTemplate(SlotModel slotModel, CustomerModel cancelledByCustomerModel)
        {
            var slotSchedulerCancelledByViewModel = CreateSlotSchedulerCancelledViewModel(slotModel, cancelledByCustomerModel);
            if (!Engine.Razor.IsTemplateCached(TemplateConstants.SlotCancelledEmailTemplateKey, typeof(SlotSchedulerBookedViewModel)))
            {
                var slotCancelledTemplateBody = GetTemplateBody(TemplateConstants.TemplateSlotCancelledNotification);
                var compiledMessageBody = Engine.Razor.RunCompile(slotCancelledTemplateBody, TemplateConstants.SlotCancelledEmailTemplateKey, typeof(SlotSchedulerCancelledViewModel), slotSchedulerCancelledByViewModel);
                return CreateEmailModel(cancelledByCustomerModel, TemplateConstants.SlotCancelledEmailSubject, compiledMessageBody);
            }

            var messageBody = Engine.Razor.Run(TemplateConstants.SlotCancelledEmailTemplateKey, typeof(SlotSchedulerCancelledViewModel), slotSchedulerCancelledByViewModel);
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

      
        //private static string GetTemplateBody(string templateName)
        //{
        //    var directory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        //    var templatesFolder = Path.Join(directory, TemplateConstants.Templates);
        //    var templatePath = Path.Join(templatesFolder, templateName);

        //    var content = string.Empty;
        //    using (var streamReader = new StreamReader(templatePath))
        //    {
        //        content = streamReader.ReadToEnd();
        //    }

        //    return content;
        //}


        private static string GetTemplateBody(string templateName)
        {
            if(templateName == TemplateConstants.TemplateCustomerRegistationWelcomeEmail)
            return TemplateBodyConstants.CustomerRegistrationWelcomeEmailTemplateBody;

            else if(templateName == TemplateConstants.TemplateSlotScheduledNotification)
            {
                return TemplateBodyConstants.SlotScheduledTemplateBody;
            }

            else if (templateName == TemplateConstants.TemplateSlotCancelledNotification)
            {
                return TemplateBodyConstants.SlotCancelledTemplateBody;
            }

            
            return "";
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

        private static RegisterCustomerViewModel CreateRegisterCustomerViewModel(CustomerModel customerModel)
        {
            var registerCustomerViewModel = new RegisterCustomerViewModel();
            registerCustomerViewModel.FirstName = customerModel.FirstName;
            registerCustomerViewModel.LastName = customerModel.LastName;
            return registerCustomerViewModel;
        }


        private static SlotSchedulerBookedViewModel CreateSlotSchedulerBookedViewModel(SlotModel slotModel, CustomerModel bookedBy)
        {
            var slotSchedulerBookedViewModel = new SlotSchedulerBookedViewModel();
            slotSchedulerBookedViewModel.Title = slotModel.Title;
            slotSchedulerBookedViewModel.Country = slotModel.Country;
            slotSchedulerBookedViewModel.TimeZone = slotModel.SlotStartZonedDateTime.Zone.Id;
            slotSchedulerBookedViewModel.SlotDate = NodaTimeHelper.FormatLocalDate(slotModel.SlotStartZonedDateTime.Date, DateTimeConstants.ApplicationOutPutDatePattern);
            slotSchedulerBookedViewModel.StartTime = slotModel.SlotStartTime.ToString();
            slotSchedulerBookedViewModel.EndTime = slotModel.SlotEndTime.ToString();
            slotSchedulerBookedViewModel.Duration = slotModel.SlotDuration.TotalMinutes.ToString();
            slotSchedulerBookedViewModel.BookedBy = string.Format("{0} {1}", bookedBy.FirstName, bookedBy.LastName);

            return slotSchedulerBookedViewModel;
        }


        private static SlotSchedulerCancelledViewModel CreateSlotSchedulerCancelledViewModel(SlotModel slotModel, CustomerModel cancelledBy)
        {
            var slotSchedulerCancelledViewModel = new SlotSchedulerCancelledViewModel();
            slotSchedulerCancelledViewModel.Title = slotModel.Title;
            slotSchedulerCancelledViewModel.Country = slotModel.Country;
            slotSchedulerCancelledViewModel.TimeZone = slotModel.SlotStartZonedDateTime.Zone.Id;
            slotSchedulerCancelledViewModel.SlotDate = NodaTimeHelper.FormatLocalDate(slotModel.SlotStartZonedDateTime.Date, DateTimeConstants.ApplicationOutPutDatePattern);
            slotSchedulerCancelledViewModel.StartTime = slotModel.SlotStartTime.ToString();
            slotSchedulerCancelledViewModel.EndTime = slotModel.SlotEndTime.ToString();
            slotSchedulerCancelledViewModel.Duration = slotModel.SlotDuration.TotalMinutes.ToString();
            slotSchedulerCancelledViewModel.CancelledBy = string.Format("{0} {1}", cancelledBy.FirstName, cancelledBy.LastName);

            return slotSchedulerCancelledViewModel;
        }
    }
}
