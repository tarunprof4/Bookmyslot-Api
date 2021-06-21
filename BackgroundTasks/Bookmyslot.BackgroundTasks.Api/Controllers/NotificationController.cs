using Bookmyslot.Api.Web.Common;
using Bookmyslot.BackgroundTasks.Api.Contracts;
using Bookmyslot.BackgroundTasks.Api.Contracts.Interfaces.Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Bookmyslot.BackgroundTasks.Api.Controllers
{
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    public class NotificationController : BaseApiController
    {
        private readonly INotificationBusiness notificationBusiness;
        public NotificationController(INotificationBusiness notificationBusiness)
        {
            this.notificationBusiness = notificationBusiness;
        }


        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("api/v1/Notification")]
        [HttpPost()]
        [ActionName("SendNotification")]
        public async Task<IActionResult> SendNotification()
        {
            CustomerModel customerModel = GetCustomerModel();
            var slotModel = GetSlotModel();

            //RegisterCustomerIntegrationEvent
            var notificationResponse = await this.notificationBusiness.SendCustomerRegisteredNotification(customerModel);

            //var notificationResponse1 = await this.notificationBusiness.SlotCancelledNotificatiion(slotModel, customerModel, customerModel);
            //var notificationResponse2 = await this.notificationBusiness.SlotMeetingInformationNotification(slotModel, customerModel);
            //var notificationResponse3 = await this.notificationBusiness.SlotScheduledNotificatiion(slotModel, customerModel, customerModel);

            return this.CreatePostHttpResponse(notificationResponse);
        }

        private CustomerModel GetCustomerModel()
        {
            return new CustomerModel()
            {
                FirstName = "First",
                LastName = "Last",
                Email = "tarun.aggarwal4@gmail.com"
            };
        }


        private SlotModel GetSlotModel()
        {
            return new SlotModel()
            {
                Title = "Title",
                Country = "Count",
                TimeZone = "time",
                SlotDate = "22-Ju-202",
                SlotStartTime = new TimeSpan(1,1,1),
                SlotEndTime = new TimeSpan(2, 2, 2),
                SlotMeetingLink = "SlotMeetingLin"
            };
        }

    }
}
