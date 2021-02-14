﻿using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.Web.Common;
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
        /// Gets profile settings by email
        /// </summary>
        /// <param name="email">customer email id</param>
        /// <returns >customer details</returns>
        /// <response code="200">Returns customer details</response>
        /// <response code="404">no customer found</response>
        /// <response code="400">validation error bad request</response>
        /// <response code="500">internal server error</response>
        // GET api/<CustomerController>/email
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{email}")]
        [ActionName("GetProfileSettings")]
        public async Task<IActionResult> Get(string email)
        {
            var customerResponse = await this.profileSettingsBusiness.GetProfileSettingsByEmail(email);
            return this.CreateGetHttpResponse(customerResponse);
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
        [ActionName("UpdateProfileSettings")]
        public async Task<IActionResult> Put([FromBody] ProfileSettingsModel profileSettingsModel)
        {
            var customerId = "29645471f47c4555918da55aed49b23a";
            var customerResponse = await this.profileSettingsBusiness.UpdateProfileSettings(profileSettingsModel, customerId);
            return this.CreatePutHttpResponse(customerResponse);
        }
    }
}
