using Bookmyslot.Api.Common;
using Bookmyslot.Api.Common.Compression.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        public async Task<IActionResult> GetCustomerYetToBeBookedSlots()
        {
            Log.Information("Get customer YetToBeBookedSlots");
            var customerSharedSlotModels = await this.customerSharedSlotBusiness.GetCustomerYetToBeBookedSlots();
            if (customerSharedSlotModels.ResultType == ResultType.Success)
            {
                HideUncessaryDetailsForGetCustomerYetToBeBookedSlots(customerSharedSlotModels.Result);
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
        [Route("api/v1/CustomerSlot/GetCustomerAvailableSlots")]
        [HttpGet()]
        public async Task<IActionResult> GetCustomerBookedSlots()
        {
            Log.Information("Get customer GetCustomerBookedSlots");
            var customerSharedSlotModels = await this.customerSharedSlotBusiness.GetCustomerBookedSlots();
            if (customerSharedSlotModels.ResultType == ResultType.Success)
            {
                HideUncessaryDetailsForGetCustomerYetToBeBookedSlots(customerSharedSlotModels.Result);
            }
            return this.CreateGetHttpResponse(customerSharedSlotModels);
        }

        private void HideUncessaryDetailsForGetCustomerYetToBeBookedSlots(IEnumerable<SharedSlotModel> sharedSlotModels)
        {
            foreach (var sharedSlotModel in sharedSlotModels)
            {
                sharedSlotModel.BookedByCustomerModel.Id = string.Empty;
                sharedSlotModel.BookedByCustomerModel.Email = string.Empty;
                sharedSlotModel.BookedByCustomerModel.Gender = string.Empty;

                sharedSlotModel.SlotModel.BookedBy = string.Empty;
            }
        }
    }
}
