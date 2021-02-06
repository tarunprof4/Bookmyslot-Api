using Bookmyslot.Api.Common.Contracts.Constants;

namespace Bookmyslot.Api.Customers.Emails
{
    public class TemplateConstants
    {
        public const string CustomerRegistrationWelcomeEmailTemplateKey = "CustomerRegistrationWelcomeEmailTemplateKey";
        public const string CustomerRegistrationWelcomeEmailSubject = AppBusinessMessagesConstants.ApplicationName + " - " + "Welcome mail";

        public const string SlotScheduledEmailTemplateKey = "SlotScheduledTemplateKey";
        public const string SlotScheduledEmailSubject = AppBusinessMessagesConstants.ApplicationName + " - " + "Slot Booked Notification";


        public const string SlotCancelledEmailTemplateKey = "SlotCancelledEmailTemplateKey";
        public const string SlotCancelledEmailSubject = AppBusinessMessagesConstants.ApplicationName + " - " + "Slot Cancelled Notification";

        public const string ResendSlotInformationEmailTemplateKey = "ResendSlotInformationTemplateKey";
        public const string ResendSlotInformationEmailSubject = AppBusinessMessagesConstants.ApplicationName + " - " + "Slot Meeting Details";


    }
}
