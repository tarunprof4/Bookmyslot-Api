using Bookmyslot.Api.Common.Compression.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.Web.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bookmyslot.Api.Controllers
{
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    [Authorize]
    public class SlotController : BaseApiController
    {
        private readonly ISlotBusiness slotBusiness;
        private readonly IKeyEncryptor keyEncryptor;

        public SlotController(ISlotBusiness slotBusiness, IKeyEncryptor keyEncryptor)
        {
            this.slotBusiness = slotBusiness;
            this.keyEncryptor = keyEncryptor;
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
        [Route("api/v1/Slot")]
        [ActionName("CreateSlot")]
        public async Task<IActionResult> Post([FromBody] SlotModel slotModel)
        {
            var slotResponse = await slotBusiness.CreateSlot(slotModel);
            return this.CreatePostHttpResponse(slotResponse);
        }

       

        /// <summary>
        /// Cancel User slot
        /// </summary>
        /// <param name="cancelSlot">user slot information</param>
        /// <returns >success or failure bool</returns>
        /// <response code="201">Returns success or failure bool</response>
        /// <response code="400">validation error bad request</response>
        /// <response code="404">no slot found</response>
        /// <response code="500">internal server error</response>
        // DELETE api/<SlotController>/email
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost()]
        [Route("api/v1/Slot/CancelSlot")]

        public async Task<IActionResult> CancelSlot([FromBody] CancelSlot cancelSlot)
        {
            var slotModel = JsonConvert.DeserializeObject<SlotModel>(this.keyEncryptor.Decrypt(cancelSlot.SlotKey));

            if (slotModel != null)
            {
                var slotResponse = await slotBusiness.CancelSlot(slotModel.Id, cancelSlot.CancelledBy);
                return this.CreatePostHttpResponse(slotResponse);
            }

            var validationErrorResponse = Response<bool>.ValidationError(new List<string>() { AppBusinessMessagesConstants.CorruptData });
            return this.CreatePostHttpResponse(validationErrorResponse);
        }
    }


   
}
