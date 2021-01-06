using Bookmyslot.Api.Common;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Controllers
{
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    public class CustomerSlotController : BaseApiController
    {
        private readonly ICustomerSlotBusiness customerSlotBusiness;
      
        public CustomerSlotController(ICustomerSlotBusiness customerSlotBusiness)
        {
            this.customerSlotBusiness = customerSlotBusiness;
        }

        [HttpGet()]
        public async Task<IActionResult> Get([FromQuery] PageParameterModel pageParameterModel)
        {
            Log.Information("Get all distinct customers nearest single slot");
            var customerSlotModels = await this.customerSlotBusiness.GetDistinctCustomersNearestSlotFromToday(pageParameterModel);
            return this.CreateGetHttpResponse(customerSlotModels);
        }

    }
}
