using Bookmyslot.Api.Common;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    public class ProfileSettingsController : BaseApiController
    {
        private readonly IProfileSettingsBusiness profileSettingsBusiness;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileSettingsController"/> class. 
        /// </summary>
        /// <param name="profileSettingsBusiness">profileSettings Business</param>
        public ProfileSettingsController(IProfileSettingsBusiness profileSettingsBusiness)
        {
            this.profileSettingsBusiness = profileSettingsBusiness;
        }

        /// <summary>
        /// Update existing customer
        /// </summary>
        /// <param name="profileSettingsModel">profileSettings model</param>
        /// <returns>success or failure bool</returns>
        /// <response code="204">Returns success or failure bool</response>
        /// <response code="400">validation error bad request</response>
        /// <response code="404">no customer found</response>
        /// <response code="500">internal server error</response>
        // PUT api/<CustomerController>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ProfileSettingsModel profileSettingsModel)
        {
            var customerId = string.Empty;
            var customerResponse = await this.profileSettingsBusiness.UpdateProfileSettings(profileSettingsModel, customerId);
            return this.CreatePutHttpResponse(customerResponse);
        }
    }
}
