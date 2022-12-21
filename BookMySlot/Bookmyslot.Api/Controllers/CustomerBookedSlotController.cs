using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Common.Web.Filters;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.ViewModels;
using Bookmyslot.Api.SlotScheduler.ViewModels.Adaptors.ResponseAdaptors.Interfaces;
using Bookmyslot.Api.Web.Common;
using Bookmyslot.SharedKernel;
using Bookmyslot.SharedKernel.Contracts.Encryption;
using Bookmyslot.SharedKernel.ValueObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Controllers
{
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    [Authorize]
    [ServiceFilter(typeof(AuthorizedFilter))]
    public class CustomerBookedSlotController : BaseApiController
    {
        private readonly ICustomerBookedSlotBusiness customerBookedSlotBusiness;
        private readonly ISymmetryEncryption symmetryEncryption;
        private readonly ICurrentUser currentUser;
        private readonly ICustomerResponseAdaptor customerResponseAdaptor;
        private readonly ICancelledSlotResponseAdaptor cancelledSlotResponseAdaptor;
        private readonly IBookedSlotResponseAdaptor bookedSlotResponseAdaptor;



        public CustomerBookedSlotController(ICustomerBookedSlotBusiness customerBookedSlotBusiness, ISymmetryEncryption symmetryEncryption, ICurrentUser currentUser, ICustomerResponseAdaptor customerResponseAdaptor, ICancelledSlotResponseAdaptor cancelledSlotResponseAdaptor, IBookedSlotResponseAdaptor bookedSlotResponseAdaptor)
        {
            this.customerBookedSlotBusiness = customerBookedSlotBusiness;
            this.symmetryEncryption = symmetryEncryption;
            this.currentUser = currentUser;
            this.customerResponseAdaptor = customerResponseAdaptor;
            this.cancelledSlotResponseAdaptor = cancelledSlotResponseAdaptor;
            this.bookedSlotResponseAdaptor = bookedSlotResponseAdaptor;
        }


        /// <summary>
        /// Gets customer booked slots
        /// </summary>
        /// <returns>returns customer booked slots</returns>
        /// <response code="200">Returns customer booked slots information</response>
        /// <response code="404">no slots found</response>
        /// <response code="400">validation error</response>
        /// <response code="500">internal server error</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("api/v1/CustomerBookedSlot/GetCustomerBookedSlots")]
        [HttpGet()]
        public async Task<IActionResult> GetCustomerBookedSlots()
        {
            var currentUserResponse = await this.currentUser.GetCurrentUserFromCache();
            var customerId = currentUserResponse.Value.Id;
            var customerBookedSlotModels = await this.customerBookedSlotBusiness.GetCustomerBookedSlots(customerId);
            var customerBookedViewModelResponse = this.bookedSlotResponseAdaptor.CreateBookedSlotViewModel(customerBookedSlotModels);
            return this.CreateGetHttpResponse(customerBookedViewModelResponse);
        }


        /// <summary>
        /// Gets customer completed slots
        /// </summary>
        /// <returns>returns customer completed slots</returns>
        /// <response code="200">Returns customer completed slot information</response>
        /// <response code="404">no slots found</response>
        /// <response code="400">validation error</response>
        /// <response code="500">internal server error</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("api/v1/CustomerBookedSlot/GetCustomerCompletedSlots")]
        [HttpGet()]



        public async Task<IActionResult> GetCustomerCompletedSlots()
        {
            var currentUserResponse = await this.currentUser.GetCurrentUserFromCache();
            var customerId = currentUserResponse.Value.Id;
            var customerBookedSlotModels = await this.customerBookedSlotBusiness.GetCustomerCompletedSlots(customerId);
            var customerBookedViewModelResponse = this.bookedSlotResponseAdaptor.CreateBookedSlotViewModel(customerBookedSlotModels);
            return this.CreateGetHttpResponse(customerBookedViewModelResponse);
        }



        /// <summary>
        /// Gets customer cancelled slots
        /// </summary>
        /// <returns>returns customer cancelled slot model</returns>
        /// <response code="200">Returns customer cancelled slot information</response>
        /// <response code="404">no slots found</response>
        /// <response code="400">validation error</response>
        /// <response code="500">internal server error</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("api/v1/CustomerBookedSlot/GetCustomerCancelledSlots")]
        [HttpGet()]
        public async Task<IActionResult> GetCustomerCancelledSlots()
        {
            var currentUserResponse = await this.currentUser.GetCurrentUserFromCache();
            var customerId = currentUserResponse.Value.Id;
            var customercancelledSlotInformationModels = await this.customerBookedSlotBusiness.GetCustomerCancelledSlots(customerId);
            if (customercancelledSlotInformationModels.ResultType == ResultType.Success)
            {
                var cancelledSlotInformationViewModels = new Result<IEnumerable<CancelledSlotInformationViewModel>>()
                { Value = this.cancelledSlotResponseAdaptor.CreateCancelledSlotInformationViewModels(customercancelledSlotInformationModels.Value) };
                return this.CreateGetHttpResponse(cancelledSlotInformationViewModels);
            }

            return this.CreateGetHttpResponse(new Result<IEnumerable<CancelledSlotInformationViewModel>>()
            { ResultType = customercancelledSlotInformationModels.ResultType, Messages = customercancelledSlotInformationModels.Messages });
        }
    }
}
