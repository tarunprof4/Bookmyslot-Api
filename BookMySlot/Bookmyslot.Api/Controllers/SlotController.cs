﻿using Bookmyslot.Api.Common;
using Bookmyslot.Api.Common.Compression.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bookmyslot.Api.Controllers
{
    [LogAttributeFilter]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    public class SlotController : BaseApiController
    {
        private readonly ISlotBusiness slotBusiness;
        private readonly IKeyEncryptor keyEncryptor;

        public SlotController(ISlotBusiness slotBusiness, IKeyEncryptor keyEncryptor)
        {
            this.slotBusiness = slotBusiness;
            this.keyEncryptor = keyEncryptor;
        }

        //[Route("api/v1/Slot")]
        //[HttpGet()]
        //public async Task<IActionResult> Get([FromQuery] PageParameterModel pageParameterModel)
        //{
        //    Log.Information("Get all slots");
        //    var customerResponse = await slotBusiness.GetAllSlots(pageParameterModel);
        //    return this.CreateGetHttpResponse(customerResponse);
        //}


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
        [HttpGet()]
        [Route("api/v1/Slot")]
        public async Task<IActionResult> Get(Guid slotId)
        {
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
        [Route("api/v1/Slot")]
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
                var slotResponse = await slotBusiness.DeleteSlot(slotModel.Id, cancelSlot.CancelledBy);
                return this.CreatePostHttpResponse(slotResponse);
            }

            var validationErrorResponse = Response<bool>.ValidationError(new List<string>() { AppBusinessMessages.CorruptData });
            return this.CreatePostHttpResponse(validationErrorResponse);
        }
    }


   
}
