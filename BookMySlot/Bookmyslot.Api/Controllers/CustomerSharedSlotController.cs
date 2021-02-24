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
    public class CustomerSharedSlotController : BaseApiController
    {
        private readonly ICustomerSharedSlotBusiness customerSharedSlotBusiness;
        private readonly IKeyEncryptor keyEncryptor;

        public CustomerSharedSlotController(ICustomerSharedSlotBusiness customerSharedSlotBusiness, IKeyEncryptor keyEncryptor)
        {
            this.customerSharedSlotBusiness = customerSharedSlotBusiness;
            this.keyEncryptor = keyEncryptor;
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
        public async Task<IActionResult> GetCustomerYetToBeBookedSlots(string customerId)
        {
            var customerSharedSlotModels = await this.customerSharedSlotBusiness.GetCustomerYetToBeBookedSlots(customerId);
            if (customerSharedSlotModels.ResultType == ResultType.Success)
            {
                HideUncessaryDetailsForGetCustomerSharedSlots(customerSharedSlotModels.Result);
            }
            return this.CreateGetHttpResponse(customerSharedSlotModels);
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("api/v1/CustomerSharedSlot/GetCustomerBookedSlots")]
        [HttpGet()]
        public async Task<IActionResult> GetCustomerBookedSlots(string customerId)
        {
            var customerSharedSlotModels = await this.customerSharedSlotBusiness.GetCustomerBookedSlots(customerId);
            if (customerSharedSlotModels.ResultType == ResultType.Success)
            {
                HideUncessaryDetailsForGetCustomerSharedSlots(customerSharedSlotModels.Result);
            }
            return this.CreateGetHttpResponse(customerSharedSlotModels);
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
        public async Task<IActionResult> GetCustomerCompletedSlots(string customerId)
        {
            var customerSharedSlotModels = await this.customerSharedSlotBusiness.GetCustomerCompletedSlots(customerId);
            if (customerSharedSlotModels.ResultType == ResultType.Success)
            {
                HideUncessaryDetailsForGetCustomerSharedSlots(customerSharedSlotModels.Result);
            }
            return this.CreateGetHttpResponse(customerSharedSlotModels);
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
        public async Task<IActionResult> GetCustomerCancelledSlots(string customerId)
        {
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
                sharedSlotModel.SharedSlotModelInformation = this.keyEncryptor.Encrypt(JsonConvert.SerializeObject(sharedSlotModel.SlotModel));

                sharedSlotModel.SlotModel.Id = Guid.Empty;
                sharedSlotModel.SlotModel.CreatedBy = string.Empty;
                sharedSlotModel.SlotModel.BookedBy = string.Empty;

                if (sharedSlotModel.BookedByCustomerModel != null)
                {
                    sharedSlotModel.BookedByCustomerModel.Id = string.Empty;
                }
            }
        }


        private void HideUncessaryDetailsForGetCustomerCancelledSlots(IEnumerable<CancelledSlotModel> cancelledSlotModels)
        {
            foreach (var cancelledSlotModel in cancelledSlotModels)
            {
                cancelledSlotModel.Id = Guid.Empty;
                cancelledSlotModel.CreatedBy = string.Empty;
                cancelledSlotModel.CancelledBy = string.Empty;
                cancelledSlotModel.BookedBy = string.Empty;
            }
        }
    }
}
