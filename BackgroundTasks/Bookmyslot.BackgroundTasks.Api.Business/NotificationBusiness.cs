using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Email;
using Bookmyslot.BackgroundTasks.Api.Contracts;
using Bookmyslot.BackgroundTasks.Api.Contracts.Interfaces;
using Bookmyslot.BackgroundTasks.Api.Emails;
using System.Threading.Tasks;

namespace Bookmyslot.BackgroundTasks.Api.Business
{
    public class NotificationBusiness : INotificationBusiness
    {
        private readonly IEmailInteraction emailInteraction;
        public NotificationBusiness(IEmailInteraction emailInteraction)
        {
            this.emailInteraction = emailInteraction;
        }

        public async Task<Response<bool>> SendCustomerRegisteredNotification(CustomerModel customerModel)
        {
            var emailModel = CustomerEmailTemplateFactory.GetCustomerRegistrationWelcomeEmailTemplate(customerModel);
            return await this.emailInteraction.SendEmail(emailModel);
        }
    }
}
