using Bookmyslot.Api.Common;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bookmyslot.Api.SlotScheduler.Controllers
{
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    public class SlotController : BaseApiController
    {
        private readonly ISlotBusiness slotBusiness;

        public SlotController(ISlotBusiness slotBusiness)
        {
            this.slotBusiness = slotBusiness;
        }

        //[HttpGet("{pageParameterModel}")]
        [HttpGet()]
        public async Task<IActionResult> Get([FromQuery] PageParameterModel pageParameterModel)
        {
            Log.Information("Get all slots");
            var customerResponse = await slotBusiness.GetAllSlots(pageParameterModel);
            return this.CreateGetHttpResponse(customerResponse);
        }


        /// <summary>
        /// Gets slot
        /// </summary>
        /// <param name="slotId">slot id</param>
        /// <returns>returns slot model</returns>
        /// <response code="200">Returns customer slot</response>
        /// <response code="404">no slot found</response>
        /// <response code="400">slot id inappropriate</response>
        /// <response code="500">internal server error</response>
        // GET: api/<SlotController>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{slotId}")]
        public async Task<IActionResult> Get(Guid slotId)
        {
            Log.Information("Get Customer Slot " + slotId);
            var slotResponse = await slotBusiness.GetSlot(slotId);
            return this.CreateGetHttpResponse(slotResponse);
        }


        /// <summary>
        /// Create new slot for the user
        /// </summary>
        /// <param name="slotModel">slot model</param>
        /// <returns >returns email id of created slot</returns>
        /// <response code="201">Returns slot id of created slot</response>
        /// <response code="400">validation error bad request</response>
        /// <response code="500">internal server error</response>
        // POST api/<SlotController>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SlotModel slotModel)
        {
            Log.Information("Create Customer Slot " + slotModel);
            var slotResponse = await slotBusiness.CreateSlot(slotModel);
            return this.CreatePostHttpResponse(slotResponse);
        }

        /// <summary>
        /// Update existing slot
        /// </summary>
        /// <param name="slotModel">slot model</param>
        /// <returns >success or failure bool</returns>
        /// <response code="204">Returns success or failure bool</response>
        /// <response code="400">validation error bad request</response>
        /// <response code="404">no slot found</response>
        /// <response code="500">internal server error</response>
        // PUT api/<SlotController>
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[HttpPut]
        //public async Task<IActionResult> Put([FromBody] SlotModel slotModel)
        //{
        //    Log.Information("Update Customer Slot " + slotModel);
        //    var slotResponse = await slotBusiness.UpdateSlot(slotModel);
        //    return this.CreatePutHttpResponse(slotResponse);

        //}

        /// <summary>
        /// Delete User slot
        /// </summary>
        /// <param name="slotId">user slot id</param>
        /// <returns >success or failure bool</returns>
        /// <response code="204">Returns success or failure bool</response>
        /// <response code="400">validation error bad request</response>
        /// <response code="404">no slot found</response>
        /// <response code="500">internal server error</response>
        // DELETE api/<SlotController>/email
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{slotId}")]
        public async Task<IActionResult> Delete(Guid slotId)
        {
            Log.Information("Delete Customer Slot  " + slotId);
            var slotResponse = await slotBusiness.DeleteSlot(slotId);
            return this.CreateDeleteHttpResponse(slotResponse);
        }
    }
}
