using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Common.Compression.Interfaces;
using Bookmyslot.Api.Common.Contracts;
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
    public class CustomerBookedSlotController : BaseApiController
    {
        private readonly ICustomerBookedSlotBusiness customerBookedSlotBusiness;
        private readonly IKeyEncryptor keyEncryptor;
        private readonly ICurrentUser currentUser;

        public CustomerBookedSlotController(ICustomerBookedSlotBusiness customerBookedSlotBusiness, IKeyEncryptor keyEncryptor, ICurrentUser currentUser)
        {
            this.customerBookedSlotBusiness = customerBookedSlotBusiness;
            this.keyEncryptor = keyEncryptor;
            this.currentUser = currentUser;
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
            var customerId = currentUserResponse.Result;
            var customerBookedSlotModels = await this.customerBookedSlotBusiness.GetCustomerBookedSlots(customerId);
            var customerBookedViewModelResponse = CreateBookedSlotViewModel(customerBookedSlotModels);
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
            var customerId = currentUserResponse.Result;
            var customerBookedSlotModels = await this.customerBookedSlotBusiness.GetCustomerCompletedSlots(customerId);
            var customerBookedViewModelResponse = CreateBookedSlotViewModel(customerBookedSlotModels);
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
            var customerId = currentUserResponse.Result;
            var customercancelledSlotInformationModels = await this.customerBookedSlotBusiness.GetCustomerCancelledSlots(customerId);
            if (customercancelledSlotInformationModels.ResultType == ResultType.Success)
            {
                HideUncessaryDetailsForGetCustomerCancelledSlots(customercancelledSlotInformationModels.Result);
            }
            return this.CreateGetHttpResponse(customercancelledSlotInformationModels);
        }


        private void HideUncessaryDetailsForGetCustomerCancelledSlots(IEnumerable<CancelledSlotInformationModel> cancelledSlotInformationModels)
        {
            foreach (var cancelledSlotInformationModel in cancelledSlotInformationModels)
            {
                cancelledSlotInformationModel.CancelledSlotModel.Id = string.Empty;
                cancelledSlotInformationModel.CancelledSlotModel.CreatedBy = string.Empty;
                cancelledSlotInformationModel.CancelledSlotModel.CancelledBy = string.Empty;
                cancelledSlotInformationModel.CancelledSlotModel.BookedBy = string.Empty;

                cancelledSlotInformationModel.CancelledByCustomerModel.Id = string.Empty;
            }
        }


        private Response<BookedSlotViewModel> CreateBookedSlotViewModel(Response<BookedSlotModel> bookedSlotModelResponse)
        {
            if (bookedSlotModelResponse.ResultType == ResultType.Success)
            {
                var bookedSlotModel = bookedSlotModelResponse.Result;
                var bookedSlotViewModel = new BookedSlotViewModel
                {
                    ToBeBookedByCustomerCountry = bookedSlotModel.CustomerSettingsModel != null ? bookedSlotModel.CustomerSettingsModel.Country : string.Empty,
                    BookedSlotModels = new List<Tuple<CustomerViewModel, SlotInforamtionInCustomerTimeZoneModel, string>>()
                };



                foreach (var bookedSlot in bookedSlotModel.BookedSlotModels)
                {
                    var slotInformation = this.keyEncryptor.Encrypt(JsonConvert.SerializeObject(bookedSlot.Value.SlotModel));
                    var createdByCustomerViewModel = new CustomerViewModel()
                    {
                        FirstName = bookedSlot.Key.FirstName,
                        LastName = bookedSlot.Key.LastName,
                        BioHeadLine = bookedSlot.Key.BioHeadLine,
                    };

                    bookedSlotViewModel.BookedSlotModels.Add(new Tuple<CustomerViewModel, SlotInforamtionInCustomerTimeZoneModel, string>(createdByCustomerViewModel, bookedSlot.Value, slotInformation));
                }

                return new Response<BookedSlotViewModel>() { Result = bookedSlotViewModel };
            }

            return new Response<BookedSlotViewModel>()
            {
                ResultType = bookedSlotModelResponse.ResultType,
                Messages = bookedSlotModelResponse.Messages
            };
        }
    }
}
