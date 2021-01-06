using Bookmyslot.Api.Common;
using Bookmyslot.Api.Common.Compression.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.SlotScheduler.Controllers
{
    //[Route("api/v1/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    public class CustomerSlotController : BaseApiController
    {
        private readonly ICustomerSlotBusiness customerSlotBusiness;
        private readonly IKeyEncryptor keyEncryptor;

        public CustomerSlotController(ICustomerSlotBusiness customerSlotBusiness, IKeyEncryptor keyEncryptor)
        {
            this.customerSlotBusiness = customerSlotBusiness;
            this.keyEncryptor = keyEncryptor;
        }

        /// <summary>
        /// Gets customer slots
        /// </summary>
        /// <param name="pageParameterModel">pageParameterModel</param>
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
        [Route("api/v1/CustomerSlot/GetDistinctCustomersNearestSlotFromToday")]
        [HttpGet()]
        public async Task<IActionResult> GetDistinctCustomersNearestSlotFromToday([FromQuery] PageParameterModel pageParameterModel)
        {
            Log.Information("Get all distinct customers nearest single slot");
            var customerSlotModels = await this.customerSlotBusiness.GetDistinctCustomersNearestSlotFromToday(pageParameterModel);
            if(customerSlotModels.ResultType == ResultType.Success)
            {
                HideUncessaryDetailsForGetDistinctCustomersNearestSlotFromToday(customerSlotModels.Result);
            }
            return this.CreateGetHttpResponse(customerSlotModels);
        }


        /// <summary>
        /// Gets customer slots
        /// </summary>
        /// <param name="pageParameterModel">pageParameterModel</param>
        /// <param name="email">customer email id</param>
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
        public async Task<IActionResult> GetCustomerAvailableSlots([FromQuery] PageParameterModel pageParameterModel, string email)
        {
            Log.Information("Get all available slots for the customer");
            var customerSlotModels = await this.customerSlotBusiness.GetCustomerAvailableSlots(pageParameterModel, email);
            if (customerSlotModels.ResultType == ResultType.Success)
            {
                HideUncessaryDetailsForGetCustomerAvailableSlots(customerSlotModels.Result);
            }
            return this.CreateGetHttpResponse(customerSlotModels);
        }

        private void HideUncessaryDetailsForGetDistinctCustomersNearestSlotFromToday(List<CustomerSlotModel> customerSlotModels)
        {
            foreach(var customerSlotModel in customerSlotModels)
            {
                customerSlotModel.CustomerModel.Email = string.Empty;
                customerSlotModel.CustomerModel.Gender = string.Empty;

                customerSlotModel.SlotModels = new List<SlotModel>();
                customerSlotModel.Information = string.Empty;
            }
        }


        private void HideUncessaryDetailsForGetCustomerAvailableSlots(List<CustomerSlotModel> customerSlotModels)
        {
            foreach (var customerSlotModel in customerSlotModels)
            {
                customerSlotModel.CustomerModel.Email = string.Empty;
                customerSlotModel.CustomerModel.Gender = string.Empty;
                foreach (var slotModel in customerSlotModel.SlotModels)
                {
                    slotModel.CreatedBy = string.Empty;
                }
                    
                customerSlotModel.Information = string.Empty;
            }
        }
    }
}
