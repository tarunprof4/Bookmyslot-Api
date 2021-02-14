using Bookmyslot.Api.Common.Compression.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.Web.Common;
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
    public class EmailController : BaseApiController
    {

        private readonly IKeyEncryptor keyEncryptor;
        private readonly IResendSlotInformationBusiness resendSlotInformationBusiness;
        public EmailController(IKeyEncryptor keyEncryptor, IResendSlotInformationBusiness resendSlotInformationBusiness)
        {
            this.keyEncryptor = keyEncryptor;
            this.resendSlotInformationBusiness = resendSlotInformationBusiness;
        }

        //[HttpPost()]
        //[Route("api/v1/email/ResendSlotMeetingInformation1")]
        //public async Task<IActionResult> ResendSlotMeetingInformation1([FromBody] ResendSlotInformation resendSlotInformation)
        //{
        //    var resendSlotInformationResponse = await this.resendSlotInformationBusiness.ResendSlotMeetingInformation(null, resendSlotInformation.ResendTo);
        //    return this.CreatePostHttpResponse(resendSlotInformationResponse);
        //}


        /// <summary>
        /// Resend slot information email to the customers
        /// </summary>
        /// <param name="resendSlotInformation">slot information</param>
        /// <returns >success or failure bool</returns>
        /// <response code="201">Returns success or failure bool</response>
        /// <response code="400">validation error bad request</response>
        /// <response code="404">no slot found</response>
        /// <response code="500">internal server error</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost()]
        [Route("api/v1/email/ResendSlotMeetingInformation")]

        public async Task<IActionResult> ResendSlotMeetingInformation([FromBody] ResendSlotInformation resendSlotInformation)
        {
            var slotModel = JsonConvert.DeserializeObject<SlotModel>(this.keyEncryptor.Decrypt(resendSlotInformation.ResendSlotModel));

            if (slotModel != null)
            {
                var resendSlotInformationResponse = await this.resendSlotInformationBusiness.ResendSlotMeetingInformation(slotModel, resendSlotInformation.ResendTo);
                return this.CreatePostHttpResponse(resendSlotInformationResponse);
            }

            var validationErrorResponse = Response<bool>.ValidationError(new List<string>() { AppBusinessMessagesConstants.CorruptData });
            return this.CreatePostHttpResponse(validationErrorResponse);
        }
    }



}
