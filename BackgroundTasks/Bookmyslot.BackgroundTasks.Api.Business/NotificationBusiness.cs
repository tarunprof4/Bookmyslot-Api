using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Email;
using Bookmyslot.BackgroundTasks.Api.Contracts;
using System;
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


        public async Task<Response<bool>> SendCustomerRegisterNotification(CustomerModel registerCustomerModel)
        {
            //this.emailInteraction.SendEmail();
            throw new NotImplementedException();
        }
    }
}
