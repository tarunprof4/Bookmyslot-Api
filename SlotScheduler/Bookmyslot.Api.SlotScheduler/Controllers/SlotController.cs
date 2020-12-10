using Bookmyslot.Api.Common;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bookmyslot.Api.SlotScheduler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlotController : BaseApiController
    {
        private readonly ISlotBusiness slotBusiness;

        public SlotController(ISlotBusiness slotBusiness)
        {
            this.slotBusiness = slotBusiness;
        }

        //[HttpGet("{email}")]
        //public async Task<IActionResult> Get(Date)
        //{
        //    Log.Information("Get all Customers");
        //    var customerResponse = await slotBusiness.GetAllSlotsDateRange();
        //    return this.CreateGetHttpResponse(customerResponse);
        //}


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            Log.Information("Get Customer Slot " + id);
            var slotResponse = await slotBusiness.GetSlot(id);
            return this.CreateGetHttpResponse(slotResponse);
        }


        // POST api/<CustomerController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SlotModel slotModel)
        {
            Log.Information("Create Customer Slot " + slotModel);
            var slotResponse = await slotBusiness.CreateSlot(slotModel);
            return this.CreatePostHttpResponse(slotResponse);
        }

        // PUT api/<CustomerController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] SlotModel slotModel)
        {
            Log.Information("Update Customer Slot " + slotModel);
            var slotResponse = await slotBusiness.UpdateSlot(slotModel);
            return this.CreatePutHttpResponse(slotResponse);

        }

        // DELETE api/<CustomerController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            Log.Information("Delete Customer Slot  " + id);
            var slotResponse = await slotBusiness.DeleteSlot(id);
            return this.CreateDeleteHttpResponse(slotResponse);
        }
    }
}
