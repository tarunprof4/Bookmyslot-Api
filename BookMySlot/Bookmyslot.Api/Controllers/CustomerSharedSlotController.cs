using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Encryption;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.ViewModels;
using Bookmyslot.Api.Web.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Controllers
{
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    [Authorize]
    public class CustomerSharedSlotController : BaseApiController
    {
        private readonly ICustomerSharedSlotBusiness customerSharedSlotBusiness;
        private readonly ISymmetryEncryption symmetryEncryption;
        private readonly ICurrentUser currentUser;

        public CustomerSharedSlotController(ICustomerSharedSlotBusiness customerSharedSlotBusiness, ISymmetryEncryption symmetryEncryption, ICurrentUser currentUser)
        {
            this.customerSharedSlotBusiness = customerSharedSlotBusiness;
            this.symmetryEncryption = symmetryEncryption;
            this.currentUser = currentUser;
        }

        /// <summary>
        /// Gets customer YetToBeBookedSlots
        /// </summary>
        /// <returns>returns customer  YetToBeBookedSlots</returns>
        /// <response code="200">Returns customer  YetToBeBookedSlots information</response>
        /// <response code="404">no slots found</response>
        /// <response code="400">validation error</response>
        /// <response code="500">internal server error</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("api/v1/CustomerSharedSlot/GetCustomerYetToBeBookedSlots")]
        [HttpGet()]
        public async Task<IActionResult> GetCustomerYetToBeBookedSlots()
        {
            var currentUserResponse = await this.currentUser.GetCurrentUserFromCache();
            var customerId = currentUserResponse.Result.Id;

            var customerSharedSlotModels = await this.customerSharedSlotBusiness.GetCustomerYetToBeBookedSlots(customerId);

            var sharedSlotViewModel = CreateBookedSlotViewModel(customerSharedSlotModels);
            return this.CreateGetHttpResponse(sharedSlotViewModel);
        }


        /// <summary>
        /// Gets customer BookedSlots
        /// </summary>
        /// <returns>returns customer BookedSlots  model</returns>
        /// <response code="200">Returns customer BookedSlots information</response>
        /// <response code="404">no slots found</response>
        /// <response code="400">validation error</response>
        /// <response code="500">internal server error</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("api/v1/CustomerSharedSlot/GetCustomerBookedSlots")]
        [HttpGet()]
        public async Task<IActionResult> GetCustomerBookedSlots()
        {
            var currentUserResponse = await this.currentUser.GetCurrentUserFromCache();
            var customerId = currentUserResponse.Result.Id;

            var customerSharedSlotModels = await this.customerSharedSlotBusiness.GetCustomerBookedSlots(customerId);

            var sharedSlotViewModel = CreateBookedSlotViewModel(customerSharedSlotModels);
            return this.CreateGetHttpResponse(sharedSlotViewModel);
        }


        /// <summary>
        /// Gets customer CompletedSlots
        /// </summary>
        /// <returns>returns customer CompletedSlots</returns>
        /// <response code="200">Returns customer CompletedSlots information</response>
        /// <response code="404">no slots found</response>
        /// <response code="400">validation error</response>
        /// <response code="500">internal server error</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("api/v1/CustomerSharedSlot/GetCustomerCompletedSlots")]
        [HttpGet()]
        public async Task<IActionResult> GetCustomerCompletedSlots()
        {
            var currentUserResponse = await this.currentUser.GetCurrentUserFromCache();
            var customerId = currentUserResponse.Result.Id;

            var customerSharedSlotModels = await this.customerSharedSlotBusiness.GetCustomerCompletedSlots(customerId);

            var sharedSlotViewModel = CreateBookedSlotViewModel(customerSharedSlotModels);
            return this.CreateGetHttpResponse(sharedSlotViewModel);
        }



        /// <summary>
        /// Gets customer CancelledSlots
        /// </summary>
        /// <returns>returns customer CancelledSlots model</returns>
        /// <response code="200">Returns customer CancelledSlots information</response>
        /// <response code="404">no slots found</response>
        /// <response code="400">validation error</response>
        /// <response code="500">internal server error</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("api/v1/CustomerSharedSlot/GetCustomerCancelledSlots")]
        [HttpGet()]
        public async Task<IActionResult> GetCustomerCancelledSlots()
        {
            var currentUserResponse = await this.currentUser.GetCurrentUserFromCache();
            var customerId = currentUserResponse.Result.Id;

            var cancelledSlotModels = await this.customerSharedSlotBusiness.GetCustomerCancelledSlots(customerId);
            if (cancelledSlotModels.ResultType == ResultType.Success)
            {
                var cancelledSlotViewModelResponse = new Response<IEnumerable<CancelledSlotViewModel>>() { Result = CancelledSlotViewModel.CreateCancelledSlotViewModels(cancelledSlotModels.Result) };
                return this.CreateGetHttpResponse(cancelledSlotViewModelResponse);
            }

            return this.CreateGetHttpResponse(new Response<IEnumerable<CancelledSlotViewModel>>()
            { ResultType = cancelledSlotModels.ResultType, Messages = cancelledSlotModels.Messages });
        }

        private Response<SharedSlotViewModel> CreateBookedSlotViewModel(Response<SharedSlotModel> sharedSlotModelResponse)
        {
            if (sharedSlotModelResponse.ResultType == ResultType.Success)
            {
                var sharedSlotModel = sharedSlotModelResponse.Result;

                var sharedSlotViewModel = new SharedSlotViewModel();
                foreach (var sharedSlot in sharedSlotModel.SharedSlotModels)
                {
                    var slotInformation = this.symmetryEncryption.Encrypt(JsonConvert.SerializeObject(sharedSlot.Value));
                    var bookedByCustomerViewModel = sharedSlot.Key != null ? CustomerViewModel.CreateCustomerViewModel(sharedSlot.Key, this.symmetryEncryption.Encrypt) : null;
                    sharedSlotViewModel.SharedSlotModels.Add(new Tuple<CustomerViewModel, SlotModel, string>(bookedByCustomerViewModel, sharedSlot.Value, slotInformation));
                }

                return new Response<SharedSlotViewModel>() { Result = sharedSlotViewModel };
            }

            return new Response<SharedSlotViewModel>()
            {
                ResultType = sharedSlotModelResponse.ResultType,
                Messages = sharedSlotModelResponse.Messages
            };
        }


       
    }
}
