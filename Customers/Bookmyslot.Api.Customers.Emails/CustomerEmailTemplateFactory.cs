using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts;
using RazorEngine;
using RazorEngine.Templating;

namespace Bookmyslot.Api.Customers.Emails
{
    public static class CustomerEmailTemplateFactory
    {

        public static string GetBookedBySlotSchedulerTemplate(SlotModel slotModel, CustomerModel bookedBy)
        {
            if (!Engine.Razor.IsTemplateCached(TemplateConstants.ResendSlotInformationTemplateKey, typeof(CustomerModel)))
            {
                return Engine.Razor.RunCompile(TemplateConstants.ResendSlotInformationTemplateBody, TemplateConstants.ResendSlotInformationTemplateKey, typeof(CustomerModel), bookedBy);
            }

            return Engine.Razor.Run(TemplateConstants.ResendSlotInformationTemplateKey, typeof(CustomerModel), bookedBy);
        }

        public static string GetResendSlotInformationTemplate(SlotModel slotModel, CustomerModel resendTo)
        {
            if (!Engine.Razor.IsTemplateCached(TemplateConstants.ResendSlotInformationTemplateKey, typeof(CustomerModel)))
            {
                return Engine.Razor.RunCompile(TemplateConstants.ResendSlotInformationTemplateBody, TemplateConstants.ResendSlotInformationTemplateKey, typeof(CustomerModel), resendTo);
            }

            return Engine.Razor.Run(TemplateConstants.ResendSlotInformationTemplateKey, typeof(CustomerModel), resendTo);
        }
    }
}
