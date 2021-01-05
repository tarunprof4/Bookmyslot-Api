using Bookmyslot.Api.Common.Contracts;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerSlotController : ControllerBase
    {
        public CustomerSlotController()
        {

        }

        [HttpGet()]
        public async Task<IActionResult> Get([FromQuery] PageParameterModel pageParameterModel)
        {
            Log.Information("Get all slots");

            return this.Ok();
            //var customerResponse = await slotBusiness.GetAllSlots(pageParameterModel);
            //return this.CreateGetHttpResponse(customerResponse);
        }

    }
}
