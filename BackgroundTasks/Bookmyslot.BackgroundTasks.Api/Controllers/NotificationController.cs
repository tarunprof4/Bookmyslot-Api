using Bookmyslot.Api.Web.Common;
using Microsoft.AspNetCore.Mvc;

namespace Bookmyslot.BackgroundTasks.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    public class NotificationController : BaseApiController
    {
        public NotificationController()
        {

        }
        
    }
}
