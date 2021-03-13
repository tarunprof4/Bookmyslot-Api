using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.Web.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    [Authorize]
    public class CustomerSettingsController : BaseApiController
    {
        private readonly ICustomerSettingsBusiness customerSettingsBusiness;
        private readonly ICurrentUser currentUser;

        public CustomerSettingsController(ICustomerSettingsBusiness customerSettingsBusiness, ICurrentUser currentUser)
        {
            this.customerSettingsBusiness = customerSettingsBusiness;
            this.currentUser = currentUser;
        }




        /// <summary>
        /// Get Customer settings
        /// </summary>
        /// <returns>customer settings details</returns>
        /// <response code="200">Returns customer settings</response>
        /// <response code="404">no settings details found</response>
        /// <response code="500">internal server error</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        [ActionName("GetCustomerAdditionalInformation")]
        public async Task<IActionResult> Get()
        {
            var currentUserResponse = await this.currentUser.GetCurrentUserFromCache();
            var customerId = currentUserResponse.Result;

            var customerSettingsResponse = await this.customerSettingsBusiness.GetCustomerSettings(customerId);
            return this.CreateGetHttpResponse(customerSettingsResponse);
        }



        /// <summary>
        /// insert or Update existing customer settings details
        /// </summary>
        /// <param name="customerSettingsModel">customer settings model</param>
        /// <returns>success or failure bool</returns>
        /// <response code="204">Returns success or failure bool</response>
        /// <response code="400">validation error bad request</response>
        /// <response code="500">internal server error</response>
        // PUT api/<CustomerController>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut]
        [ActionName("UpdateCustomerAdditionalInformation")]
        public async Task<IActionResult> Put([FromBody] CustomerSettingsModel customerSettingsModel)
        {
            var currentUserResponse = await this.currentUser.GetCurrentUserFromCache();
            var customerId = currentUserResponse.Result;
            var customerSettingsResponse = await this.customerSettingsBusiness.UpdateCustomerSettings(customerId, customerSettingsModel);
            return this.CreatePutHttpResponse(customerSettingsResponse);
        }
    }
}
