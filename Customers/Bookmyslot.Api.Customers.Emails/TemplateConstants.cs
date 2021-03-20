using Bookmyslot.Api.Common.Contracts.Constants;

namespace Bookmyslot.Api.Customers.Emails
{
    public class TemplateConstants
    {
        public const string Templates = "Templates";
        public const string TemplateCustomerRegistationWelcomeEmailNotification = "CustomerRegistationWelcomeEmailNotification.html";
        public const string TemplateSlotMeetingInformationNotification = "SlotMeetingInformationNotification.html";
        public const string TemplateSlotCancelledNotification = "SlotCancelledNotification.html";
        public const string TemplateSlotScheduledNotification = "SlotScheduledNotification.html";

        public const string CustomerRegistrationWelcomeEmailTemplateKey = "CustomerRegistrationWelcomeEmailTemplateKey";
        public const string CustomerRegistrationWelcomeEmailSubject = AppBusinessMessagesConstants.ApplicationName + " - " + "Welcome mail";

        public const string SlotScheduledEmailTemplateKey = "SlotScheduledTemplateKey";
        public const string SlotScheduledEmailSubject = AppBusinessMessagesConstants.ApplicationName + " - " + "Slot Booked Notification";


        public const string SlotCancelledEmailTemplateKey = "SlotCancelledEmailTemplateKey";
        public const string SlotCancelledEmailSubject = AppBusinessMessagesConstants.ApplicationName + " - " + "Slot Cancelled Notification";

        public const string SlotMeetingInformationEmailTemplateKey = "SlotMeetingInformationTemplateKey";
        public const string SlotMeetingInformationEmailSubject = AppBusinessMessagesConstants.ApplicationName + " - " + "Slot Meeting Details";


    }
}
