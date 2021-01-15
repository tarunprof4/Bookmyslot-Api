namespace Bookmyslot.Api.Customers.Emails
{
    public class TemplateBodyConstants
    {

        public const string CustomerRegistrationWelcomeEmailTemplateBody = @"<!DOCTYPE html>
<html>
<head>
    
</head>
<body style = 'font-family: Arial; font-size: 14px;' >
    <div>
        Hi @Model.FirstName! Welcome to BookMyslot.
    </div>
</body>
</html>";

        public const string SlotScheduledTemplateBody = "";

        public const string ResendSlotInformationTemplateBody = "Hello @Model.FirstName, welcome to RazorEngine!";

        public const string SlotCancelledTemplateBody = "";
    }
}
