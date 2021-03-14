using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.Customers.ViewModels;
using Bookmyslot.Api.Customers.ViewModels.Validations;
using Bookmyslot.Api.Location.Interfaces;
using Bookmyslot.Api.Web.Common;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
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
        private readonly INodaTimeZoneLocationBusiness nodaTimeZoneLocationBusiness;

        public CustomerSettingsController(ICustomerSettingsBusiness customerSettingsBusiness, ICurrentUser currentUser, INodaTimeZoneLocationBusiness nodaTimeZoneLocationBusiness)
        {
            this.customerSettingsBusiness = customerSettingsBusiness;
            this.currentUser = currentUser;
            this.nodaTimeZoneLocationBusiness = nodaTimeZoneLocationBusiness;
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
        /// <param name="customerSettingsViewModel">customer settings model</param>
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
        public async Task<IActionResult> Put([FromBody] CustomerSettingsViewModel customerSettingsViewModel)
        {
            var validator = new CustomerSettingsViewModelValidator(this.nodaTimeZoneLocationBusiness);
            ValidationResult results = validator.Validate(customerSettingsViewModel);

            if (results.IsValid)
            {
                var currentUserResponse = await this.currentUser.GetCurrentUserFromCache();
                var customerId = currentUserResponse.Result;
                var customerSettingsResponse = await this.customerSettingsBusiness.UpdateCustomerSettings(customerId, CreateCustomerSettingsViewModel(customerSettingsViewModel));
                return this.CreatePutHttpResponse(customerSettingsResponse);
            }
            var validationResponse = Response<bool>.ValidationError(results.Errors.Select(a => a.ErrorMessage).ToList());
            return this.CreatePutHttpResponse(validationResponse);
        }

        private CustomerSettingsModel CreateCustomerSettingsViewModel(CustomerSettingsViewModel customerSettingsViewModel)
        {
            return new CustomerSettingsModel() { TimeZone = customerSettingsViewModel.TimeZone };
        }
    }
}
