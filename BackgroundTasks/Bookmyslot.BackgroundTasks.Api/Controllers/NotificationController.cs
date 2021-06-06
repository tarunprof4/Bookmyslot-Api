using Bookmyslot.Api.Web.Common;
using Bookmyslot.BackgroundTasks.Api.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Bookmyslot.BackgroundTasks.Api.Controllers
{
    [Route("api/[controller]")]
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
            var registerCustomerModel = new CustomerModel()
            {
                FirstName = "First",
                LastName = "Last",
                Email = "tarun.aggarwal4@gmail.com"
            };

            var notificationResponse = await this.notificationBusiness.SendCustomerRegisterNotification(registerCustomerModel);

            return this.CreatePostHttpResponse(notificationResponse);
        }
    }
}
