using System;
using System.Collections.Generic;
using System.Text;

namespace Bookmyslot.Api.Customers.Emails
{
    public class TemplateConstants
    {
        public const string ResendSlotInformationTemplateKey = "ResendSlotInformationTemplateKey";
        public const string ResendSlotInformationTemplate = "Hello @Model.FirstName, welcome to RazorEngine!";
    }
}
