using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Encryption;
using Bookmyslot.Api.Common.Web.Filters;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.Api.SlotScheduler.ViewModels;
using Bookmyslot.Api.Web.Common;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bookmyslot.Api.Controllers
{

    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    [Authorize]
    [ServiceFilter(typeof(AuthorizedFilter))]
    public class EmailController : BaseApiController
    {
        private readonly ISymmetryEncryption symmetryEncryption;
        private readonly IResendSlotInformationBusiness resendSlotInformationBusiness;
        private readonly ICurrentUser currentUser;
        private readonly IValidator<ResendSlotInformationViewModel> resendSlotInformationViewModelValidator;
        public EmailController(ISymmetryEncryption symmetryEncryption, IResendSlotInformationBusiness resendSlotInformationBusiness, 
            ICurrentUser currentUser, IValidator<ResendSlotInformationViewModel> resendSlotInformationViewModelValidator)
        {
            this.symmetryEncryption = symmetryEncryption;
            this.resendSlotInformationBusiness = resendSlotInformationBusiness;
            this.currentUser = currentUser;
            this.resendSlotInformationViewModelValidator = resendSlotInformationViewModelValidator;
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
        /// <param name="resendSlotInformationViewModel">slot information</param>
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

        public async Task<IActionResult> ResendSlotMeetingInformation([FromBody] ResendSlotInformationViewModel resendSlotInformationViewModel)
        {
            ValidationResult results = this.resendSlotInformationViewModelValidator.Validate(resendSlotInformationViewModel);

            if (results.IsValid)
            {
                var slotModel = JsonConvert.DeserializeObject<SlotModel>(this.symmetryEncryption.Decrypt(resendSlotInformationViewModel.ResendSlotModel));

                if (slotModel != null)
                {
                    var currentUserResponse = await this.currentUser.GetCurrentUserFromCache();
                    var resendSlotInformationResponse = await this.resendSlotInformationBusiness.ResendSlotMeetingInformation(slotModel,
                        currentUserResponse.Result.Id);
                    return this.CreatePostHttpResponse(resendSlotInformationResponse);
                }

                var validationErrorResponse = Response<bool>.ValidationError(new List<string>() { AppBusinessMessagesConstants.CorruptData });
                return this.CreatePostHttpResponse(validationErrorResponse);
            }
            var validationResponse = Response<bool>.ValidationError(results.Errors.Select(a => a.ErrorMessage).ToList());
            return this.CreatePostHttpResponse(validationResponse);
        }

      
    }



}
