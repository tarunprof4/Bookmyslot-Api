using Bookmyslot.Api.Common.Compression.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.Web.Common;
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
    public class CustomerBookedSlotController : BaseApiController
    {
        private readonly ICustomerBookedSlotBusiness customerBookedSlotBusiness;
        private readonly IKeyEncryptor keyEncryptor;

        public CustomerBookedSlotController(ICustomerBookedSlotBusiness customerBookedSlotBusiness, IKeyEncryptor keyEncryptor)
        {
            this.customerBookedSlotBusiness = customerBookedSlotBusiness;
            this.keyEncryptor = keyEncryptor;
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("api/v1/CustomerBookedSlot/GetCustomerBookedSlots")]
        [HttpGet()]
        public async Task<IActionResult> GetCustomerBookedSlots(string customerId)
        {
            var customerBookedSlotModels = await this.customerBookedSlotBusiness.GetCustomerBookedSlots(customerId);
            if (customerBookedSlotModels.ResultType == ResultType.Success)
            {
                HideUncessaryDetailsForGetCustomerBookedSlots(customerBookedSlotModels.Result);
            }
            return this.CreateGetHttpResponse(customerBookedSlotModels);
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("api/v1/CustomerBookedSlot/GetCustomerCompletedSlots")]
        [HttpGet()]
        public async Task<IActionResult> GetCustomerCompletedSlots(string customerId)
        {
            var customerBookedSlotModels = await this.customerBookedSlotBusiness.GetCustomerCompletedSlots(customerId);
            if (customerBookedSlotModels.ResultType == ResultType.Success)
            {
                HideUncessaryDetailsForGetCustomerBookedSlots(customerBookedSlotModels.Result);
            }
            return this.CreateGetHttpResponse(customerBookedSlotModels);
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
        public async Task<IActionResult> GetCustomerCancelledSlots(string customerId)
        {
            var customercancelledSlotInformationModels = await this.customerBookedSlotBusiness.GetCustomerCancelledSlots(customerId);
            if (customercancelledSlotInformationModels.ResultType == ResultType.Success)
            {
                HideUncessaryDetailsForGetCustomerCancelledSlots(customercancelledSlotInformationModels.Result);
            }
            return this.CreateGetHttpResponse(customercancelledSlotInformationModels);
        }

        private void HideUncessaryDetailsForGetCustomerBookedSlots(IEnumerable<BookedSlotModel> bookSlotModels)
        {
            foreach (var bookSlotModel in bookSlotModels)
            {
                bookSlotModel.BookedSlotModelInformation = this.keyEncryptor.Encrypt(JsonConvert.SerializeObject(bookSlotModel.SlotModel));
                bookSlotModel.SlotModel.Id = Guid.Empty;
                bookSlotModel.SlotModel.BookedBy = string.Empty;
                bookSlotModel.SlotModel.CreatedBy = string.Empty;

                bookSlotModel.CreatedByCustomerModel.Id = string.Empty;
            }
        }

        private void HideUncessaryDetailsForGetCustomerCancelledSlots(IEnumerable<CancelledSlotInformationModel> cancelledSlotInformationModels)
        {
            foreach (var cancelledSlotInformationModel in cancelledSlotInformationModels)
            {
                cancelledSlotInformationModel.CancelledSlotModel.Id = Guid.Empty;
                cancelledSlotInformationModel.CancelledSlotModel.CreatedBy = string.Empty;
                cancelledSlotInformationModel.CancelledSlotModel.CancelledBy = string.Empty;
                cancelledSlotInformationModel.CancelledSlotModel.BookedBy = string.Empty;

                cancelledSlotInformationModel.CancelledByCustomerModel.Id = string.Empty;
            }
        }
    }
}
