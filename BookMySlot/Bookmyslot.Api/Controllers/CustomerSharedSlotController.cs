using Bookmyslot.Api.Common;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Controllers
{
    //[Route("api/v1/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    public class CustomerSharedSlotController : BaseApiController
    {
        private readonly ICustomerSharedSlotBusiness customerSharedSlotBusiness;

        public CustomerSharedSlotController(ICustomerSharedSlotBusiness customerSharedSlotBusiness)
        {
            this.customerSharedSlotBusiness = customerSharedSlotBusiness;
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
        [Route("api/v1/CustomerSharedSlot/GetCustomerYetToBeBookedSlots")]
        [HttpGet()]
        public async Task<IActionResult> GetCustomerYetToBeBookedSlots(string customerId)
        {
            Log.Information("Get customer YetToBeBookedSlots");
            var customerSharedSlotModels = await this.customerSharedSlotBusiness.GetCustomerYetToBeBookedSlots(customerId);
            if (customerSharedSlotModels.ResultType == ResultType.Success)
            {
                HideUncessaryDetailsForGetCustomerSharedSlots(customerSharedSlotModels.Result);
            }
            return this.CreateGetHttpResponse(customerSharedSlotModels);
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
        [Route("api/v1/CustomerSharedSlot/GetCustomerBookedSlots")]
        [HttpGet()]
        public async Task<IActionResult> GetCustomerBookedSlots(string customerId)
        {
            Log.Information("Get customer GetCustomerBookedSlots");
            var customerSharedSlotModels = await this.customerSharedSlotBusiness.GetCustomerBookedSlots(customerId);
            if (customerSharedSlotModels.ResultType == ResultType.Success)
            {
                HideUncessaryDetailsForGetCustomerSharedSlots(customerSharedSlotModels.Result);
            }
            return this.CreateGetHttpResponse(customerSharedSlotModels);
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
        [Route("api/v1/CustomerSharedSlot/GetCustomerCompletedSlots")]
        [HttpGet()]
        public async Task<IActionResult> GetCustomerCompletedSlots(string customerId)
        {
            Log.Information("Get customer GetCustomerCompletedSlots");
            var customerSharedSlotModels = await this.customerSharedSlotBusiness.GetCustomerCompletedSlots(customerId);
            if (customerSharedSlotModels.ResultType == ResultType.Success)
            {
                HideUncessaryDetailsForGetCustomerSharedSlots(customerSharedSlotModels.Result);
            }
            return this.CreateGetHttpResponse(customerSharedSlotModels);
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
        [Route("api/v1/CustomerSharedSlot/GetCustomerCancelledSlots")]
        [HttpGet()]
        public async Task<IActionResult> GetCustomerCancelledSlots(string customerId)
        {
            Log.Information("Get customer GetCustomerCancelledSlots");
            var cancelledSlotModels = await this.customerSharedSlotBusiness.GetCustomerCancelledSlots(customerId);
            if (cancelledSlotModels.ResultType == ResultType.Success)
            {
                HideUncessaryDetailsForGetCustomerCancelledSlots(cancelledSlotModels.Result);
            }
            return this.CreateGetHttpResponse(cancelledSlotModels);
        }

        private void HideUncessaryDetailsForGetCustomerSharedSlots(IEnumerable<SharedSlotModel> sharedSlotModels)
        {
            foreach (var sharedSlotModel in sharedSlotModels)
            {
                //sharedSlotModel.BookedByCustomerModel?.Id = string.Empty;
                //sharedSlotModel.BookedByCustomerModel.Email = string.Empty;
                //sharedSlotModel.BookedByCustomerModel.Gender = string.Empty;

                sharedSlotModel.SlotModel.BookedBy = string.Empty;
            }
        }


        private void HideUncessaryDetailsForGetCustomerCancelledSlots(IEnumerable<CancelledSlotModel> cancelledSlotModels)
        {
            foreach (var cancelledSlotModel in cancelledSlotModels)
            {
                cancelledSlotModel.CreatedBy = string.Empty;
            }
        }
    }
}
