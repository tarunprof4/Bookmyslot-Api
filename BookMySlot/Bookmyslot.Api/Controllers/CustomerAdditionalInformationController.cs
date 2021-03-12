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
    public class CustomerAdditionalInformationController : BaseApiController
    {
        private readonly ICustomerAdditionalInformationBusiness customerAdditionalInformationBusiness;
        private readonly ICurrentUser currentUser;

        public CustomerAdditionalInformationController(ICustomerAdditionalInformationBusiness customerAdditionalInformationBusiness, ICurrentUser currentUser)
        {
            this.customerAdditionalInformationBusiness = customerAdditionalInformationBusiness;
            this.currentUser = currentUser;
        }




        /// <summary>
        /// Get Customer Additional Information
        /// </summary>
        /// <returns>customer Additional Information details</returns>
        /// <response code="200">Returns customer Additional Information details</response>
        /// <response code="404">no additional details found</response>
        /// <response code="500">internal server error</response>
        // GET api/<CustomerController>/email
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        [ActionName("GetCustomerAdditionalInformation")]
        public async Task<IActionResult> Get()
        {
            var currentUserResponse = await this.currentUser.GetCurrentUserFromCache();
            var customerId = currentUserResponse.Result;

            var customerAdditionalInformationResponse = await this.customerAdditionalInformationBusiness.GetCustomerAdditionalInformation(customerId);
            return this.CreateGetHttpResponse(customerAdditionalInformationResponse);
        }



        /// <summary>
        /// Update existing customer additional details
        /// </summary>
        /// <param name="customerAdditionalInformationModel">customerAdditionalInformation model</param>
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
        public async Task<IActionResult> Put([FromBody] CustomerAdditionalInformationModel customerAdditionalInformationModel)
        {
            var currentUserResponse = await this.currentUser.GetCurrentUserFromCache();
            var customerId = currentUserResponse.Result;
            var customerAdditionalInformationResponse = await this.customerAdditionalInformationBusiness.UpdateCustomerAdditionalInformation(customerId, customerAdditionalInformationModel);
            return this.CreatePutHttpResponse(customerAdditionalInformationResponse);
        }
    }
}
