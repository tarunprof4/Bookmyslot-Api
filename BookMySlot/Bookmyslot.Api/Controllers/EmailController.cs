﻿
using Bookmyslot.Api.Common;
using Bookmyslot.Api.Common.Compression.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bookmyslot.Api.Controllers
{

    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    public class EmailController : BaseApiController
    {
        private readonly IKeyEncryptor keyEncryptor;
        public EmailController(IKeyEncryptor keyEncryptor)
        {
            this.keyEncryptor = keyEncryptor;
        }

        /// Delete User slot
        /// </summary>
        /// <param name="cancelSlot">user slot id</param>
        /// <returns >success or failure bool</returns>
        /// <response code="204">Returns success or failure bool</response>
        /// <response code="400">validation error bad request</response>
        /// <response code="404">no slot found</response>
        /// <response code="500">internal server error</response>
        // DELETE api/<SlotController>/email
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost()]
        [Route("api/v1/Email/ResendSlotInformation")]

        public async Task<IActionResult> ResendSlotInformation([FromBody] ResendSlotInformation resendSlotInformation)
        {
            Log.Information("Delete Customer Slot  " + resendSlotInformation.SlotKey);
            var slotModel = JsonConvert.DeserializeObject<SlotModel>(this.keyEncryptor.Decrypt(resendSlotInformation.SlotKey));

            if (slotModel != null)
            {
               
            }

            var validationErrorResponse = Response<bool>.ValidationError(new List<string>() { AppBusinessMessages.CorruptData });
            return this.CreatePostHttpResponse(validationErrorResponse);
        }
    }


   
}
