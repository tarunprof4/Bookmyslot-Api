namespace Bookmyslot.Api.Customers.Emails
{
    public class TemplateConstants
    {
        public const string ResendSlotInformationTemplateKey = "ResendSlotInformationTemplateKey";
        public const string ResendSlotInformationTemplateSubject = "Slot Meeting Details";
        public const string ResendSlotInformationTemplateBody = "Hello @Model.FirstName, welcome to RazorEngine!";
    }
}
