using Bookmyslot.Api.Common;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Controllers
{
    //[Route("api/v1/[controller]")]
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

        [Route("api/v1/CustomerSlot/GetDistinctCustomersNearestSlotFromToday")]
        [HttpGet()]
        public async Task<IActionResult> GetDistinctCustomersNearestSlotFromToday([FromQuery] PageParameterModel pageParameterModel)
        {
            Log.Information("Get all distinct customers nearest single slot");
            var customerSlotModels = await this.customerSlotBusiness.GetDistinctCustomersNearestSlotFromToday(pageParameterModel);
            return this.CreateGetHttpResponse(customerSlotModels);
        }

        [Route("api/v1/CustomerSlot/GetCustomerAvailableSlots")]
        [HttpGet()]
        public async Task<IActionResult> GetCustomerAvailableSlots([FromQuery] PageParameterModel pageParameterModel, string email)
        {
            Log.Information("Get all available slots for the customer");
            var customerSlotModels = await this.customerSlotBusiness.GetCustomerAvailableSlots(pageParameterModel, email);
            return this.CreateGetHttpResponse(customerSlotModels);
        }

    }
}
