using Bookmyslot.Api.Cache.Contracts.Interfaces;
using Bookmyslot.Api.Common;
using Bookmyslot.Api.Common.Compression.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Controllers
{
    //[Route("api/v1/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    public class CustomerSlotController : BaseApiController
    {
        private readonly ICustomerSlotBusiness customerSlotBusiness;
        private readonly IKeyEncryptor keyEncryptor;
        private readonly ITableCacheHandler tableCacheHandler;
        private readonly IHashing md5Hash;

        public CustomerSlotController(ICustomerSlotBusiness customerSlotBusiness, IKeyEncryptor keyEncryptor, ITableCacheHandler tableCacheHandler, IHashing md5Hash)
        {
            this.customerSlotBusiness = customerSlotBusiness;
            this.keyEncryptor = keyEncryptor;
            this.tableCacheHandler = tableCacheHandler;
            this.md5Hash = md5Hash;
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
            var key = this.md5Hash.Create(pageParameterModel);
            var customerSlotModels =
                  await
                  this.tableCacheHandler.GetFromCacheAsync(
                      key,
                      () => this.customerSlotBusiness.GetDistinctCustomersNearestSlotFromToday(pageParameterModel),
                      60);


            //var customerSlotModels = await this.customerSlotBusiness.GetDistinctCustomersNearestSlotFromToday(pageParameterModel);
            if (customerSlotModels.ResultType == ResultType.Success)
            {
                HideUncessaryDetailsForGetDistinctCustomersNearestSlotFromToday(customerSlotModels.Result);
            }
            return this.CreateGetHttpResponse(customerSlotModels);
        }


        /// <summary>
        /// Gets customer slots
        /// </summary>
        /// <param name="pageParameterModel">pageParameterModel</param>
        /// <param name="customerInfo">customer info</param>
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
        public async Task<IActionResult> GetCustomerAvailableSlots([FromQuery] PageParameterModel pageParameterModel, string customerInfo)
        {
            var bookSlotModelResponse = await this.customerSlotBusiness.GetCustomerAvailableSlots(pageParameterModel, customerInfo);
            if (bookSlotModelResponse.ResultType == ResultType.Success)
            {
                HideUncessaryDetailsForGetCustomerAvailableSlots(bookSlotModelResponse.Result);
            }
            return this.CreateGetHttpResponse(bookSlotModelResponse);
        }

        private void HideUncessaryDetailsForGetDistinctCustomersNearestSlotFromToday(List<CustomerSlotModel> customerSlotModels)
        {
            foreach (var customerSlotModel in customerSlotModels)
            {
                customerSlotModel.CustomerModel.Email = string.Empty;

                customerSlotModel.SlotModels = new List<SlotModel>();
            }
        }


        private void HideUncessaryDetailsForGetCustomerAvailableSlots(BookSlotModel bookSlotModel)
        {
            foreach (var slotModel in bookSlotModel.SlotModelsInforamtion)
            {
                slotModel.Value = this.keyEncryptor.Encrypt(JsonConvert.SerializeObject(slotModel));
                slotModel.Key.CreatedBy = string.Empty;
            }

            bookSlotModel.CustomerModel.Email = string.Empty;
        }
    }
}
