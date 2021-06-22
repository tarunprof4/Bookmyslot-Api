using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Encryption;
using Bookmyslot.Api.Common.Web.Filters;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.Api.SlotScheduler.ViewModels;
using Bookmyslot.Api.SlotScheduler.ViewModels.Adaptors.RequestAdaptors.Interfaces;
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
    public class SlotController : BaseApiController
    {
        private readonly ISlotBusiness slotBusiness;
        private readonly ISymmetryEncryption symmetryEncryption;
        private readonly ICurrentUser currentUser;
        private readonly ISlotRequestAdaptor slotRequestAdaptor;
        private readonly IValidator<SlotViewModel> slotViewModelValidator;
        private readonly IValidator<CancelSlotViewModel> cancelSlotViewModelValidator;

        public SlotController(ISlotBusiness slotBusiness, ISymmetryEncryption symmetryEncryption, ICurrentUser currentUser,
            ISlotRequestAdaptor slotRequestAdaptor, IValidator<SlotViewModel> slotViewModelValidator,
            IValidator<CancelSlotViewModel> cancelSlotViewModelValidator)
        {
            this.slotBusiness = slotBusiness;
            this.symmetryEncryption = symmetryEncryption;
            this.currentUser = currentUser;
            this.slotRequestAdaptor = slotRequestAdaptor;
            this.slotViewModelValidator = slotViewModelValidator;
            this.cancelSlotViewModelValidator = cancelSlotViewModelValidator;
        }


        /// <summary>
        /// Create new slot for the user
        /// </summary>
        /// <param name="slotViewModel">slot view model</param>
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
        public async Task<IActionResult> Post([FromBody] SlotViewModel slotViewModel)
        {
            ValidationResult results = this.slotViewModelValidator.Validate(slotViewModel);

            if (results.IsValid)
            {
                var currentUserResponse = await this.currentUser.GetCurrentUserFromCache();
                var customerId = currentUserResponse.Result.Id;

                var slotResponse = await slotBusiness.CreateSlot(this.slotRequestAdaptor.CreateSlotModel(slotViewModel), customerId);
                return this.CreatePostHttpResponse(slotResponse);
            }
            var validationResponse = Response<string>.ValidationError(results.Errors.Select(a => a.ErrorMessage).ToList());
            return this.CreatePostHttpResponse(validationResponse);
        }



        /// <summary>
        /// Cancel User slot
        /// </summary>
        /// <param name="cancelSlotViewModel">user slot information</param>
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

        public async Task<IActionResult> CancelSlot([FromBody] CancelSlotViewModel cancelSlotViewModel)
        {
            ValidationResult results = this.cancelSlotViewModelValidator.Validate(cancelSlotViewModel);

            if (results.IsValid)
            {
                var slotModel = JsonConvert.DeserializeObject<SlotModel>(this.symmetryEncryption.Decrypt(cancelSlotViewModel.SlotKey));

                if (slotModel != null)
                {
                    var currentUserResponse = await this.currentUser.GetCurrentUserFromCache();
                    var slotResponse = await slotBusiness.CancelSlot(slotModel.Id, currentUserResponse.Result.Id);
                    return this.CreatePostHttpResponse(slotResponse);
                }

                var validationErrorResponse = Response<bool>.ValidationError(new List<string>() { AppBusinessMessagesConstants.CorruptData });
                return this.CreatePostHttpResponse(validationErrorResponse);
            }
            var validationResponse = Response<bool>.ValidationError(results.Errors.Select(a => a.ErrorMessage).ToList());
            return this.CreatePostHttpResponse(validationResponse);
        }

    }
}
