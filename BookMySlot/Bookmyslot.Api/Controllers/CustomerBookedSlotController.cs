using Bookmyslot.Api.Common;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
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

        public CustomerBookedSlotController(ICustomerBookedSlotBusiness customerBookedSlotBusiness)
        {
            this.customerBookedSlotBusiness = customerBookedSlotBusiness;
        }


        /// <summary>
        /// Gets customer slots
        /// </summary>
        /// <returns>returns slot model</returns>
        /// <response code="200">Returns customer slot information</response>
        /// <response code="404">no slots found</response>
        /// <response code="400">validation error</response>
        /// <response code="500">internal server error</response>
        // GET: api/<CustomerSlot>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("api/v1/CustomerBookedSlot/GetCustomerBookedSlots")]
        [HttpGet()]
        public async Task<IActionResult> GetCustomerBookedSlots(string customerId)
        {
            Log.Information("Get customer GetCustomerBookedSlots");
            var customerBookedSlotModels = await this.customerBookedSlotBusiness.GetCustomerBookedSlots(customerId);
            if (customerBookedSlotModels.ResultType == ResultType.Success)
            {
                HideUncessaryDetailsForGetCustomerBookedSlots(customerBookedSlotModels.Result);
            }
            return this.CreateGetHttpResponse(customerBookedSlotModels);
        }


        /// <summary>
        /// Gets customer slots
        /// </summary>
        /// <returns>returns slot model</returns>
        /// <response code="200">Returns customer slot information</response>
        /// <response code="404">no slots found</response>
        /// <response code="400">validation error</response>
        /// <response code="500">internal server error</response>
        // GET: api/<CustomerSlot>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("api/v1/CustomerBookedSlot/GetCustomerCompletedSlots")]
        [HttpGet()]
        public async Task<IActionResult> GetCustomerCompletedSlots(string customerId)
        {
            Log.Information("Get customer GetCustomerCompletedSlots");
            var customerBookedSlotModels = await this.customerBookedSlotBusiness.GetCustomerCompletedSlots(customerId);
            if (customerBookedSlotModels.ResultType == ResultType.Success)
            {
                HideUncessaryDetailsForGetCustomerBookedSlots(customerBookedSlotModels.Result);
            }
            return this.CreateGetHttpResponse(customerBookedSlotModels);
        }



        /// <summary>
        /// Gets customer slots
        /// </summary>
        /// <returns>returns slot model</returns>
        /// <response code="200">Returns customer slot information</response>
        /// <response code="404">no slots found</response>
        /// <response code="400">validation error</response>
        /// <response code="500">internal server error</response>
        // GET: api/<CustomerSlot>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("api/v1/CustomerBookedSlot/GetCustomerCancelledSlots")]
        [HttpGet()]
        public async Task<IActionResult> GetCustomerCancelledSlots(string customerId)
        {
            Log.Information("Get customer GetCustomerCancelledSlots");
            var customercancelledSlotModels = await this.customerBookedSlotBusiness.GetCustomerCancelledSlots(customerId);
            if (customercancelledSlotModels.ResultType == ResultType.Success)
            {
                foreach (var customercancelledSlotModel in customercancelledSlotModels.Result)
                {
                    customercancelledSlotModel.Id = Guid.Empty;
                    customercancelledSlotModel.CreatedBy = string.Empty;
                    customercancelledSlotModel.CancelledBy = string.Empty;
                }
            }
            return this.CreateGetHttpResponse(customercancelledSlotModels);
        }

        private void HideUncessaryDetailsForGetCustomerBookedSlots(IEnumerable<BookedSlotModel> bookSlotModels)
        {
            foreach (var bookSlotModel in bookSlotModels)
            {
                bookSlotModel.CreatedByCustomerModel.Id = string.Empty;
                bookSlotModel.CreatedByCustomerModel.Email = string.Empty;
                bookSlotModel.CreatedByCustomerModel.Gender = string.Empty;

                bookSlotModel.SlotModel.BookedBy = string.Empty;
            }
        }
    }
}
