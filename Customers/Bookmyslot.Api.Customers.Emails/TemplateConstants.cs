using Bookmyslot.Api.Common.Contracts.Constants;

namespace Bookmyslot.Api.Customers.Emails
{
    public class TemplateConstants
    {
        public const string CustomerRegistrationWelcomeEmailTemplateKey = "CustomerRegistrationWelcomeEmailTemplateKey";
        public const string CustomerRegistrationWelcomeEmailSubject = AppBusinessMessages.ApplicationName + " - " + "Welcome mail";

        public const string SlotScheduledEmailTemplateKey = "SlotScheduledTemplateKey";
        public const string SlotScheduledEmailSubject = AppBusinessMessages.ApplicationName + " - " + "Slot Booked Notification";


        public const string SlotCancelledEmailTemplateKey = "SlotCancelledEmailTemplateKey";
        public const string SlotCancelledEmailSubject = AppBusinessMessages.ApplicationName + " - " + "Slot Cancelled Notification";

        public const string ResendSlotInformationEmailTemplateKey = "ResendSlotInformationTemplateKey";
        public const string ResendSlotInformationEmailSubject = AppBusinessMessages.ApplicationName + " - " + "Slot Meeting Details";


    }
}
