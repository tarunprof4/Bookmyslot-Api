using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Web.Filters;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.Customers.Domain;
using Bookmyslot.Api.Customers.ViewModels;
using Bookmyslot.Api.Web.Common;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    [Authorize]
    [ServiceFilter(typeof(AuthorizedFilter))]
    public class CustomerSettingsController : BaseApiController
    {
        private readonly ICustomerSettingsBusiness customerSettingsBusiness;
        private readonly ICurrentUser currentUser;
        private readonly IValidator<CustomerSettingsViewModel> customerSettingsViewModelValidator;

        public CustomerSettingsController(ICustomerSettingsBusiness customerSettingsBusiness, ICurrentUser currentUser,
             IValidator<CustomerSettingsViewModel> customerSettingsViewModelValidator)
        {
            this.customerSettingsBusiness = customerSettingsBusiness;
            this.currentUser = currentUser;
            this.customerSettingsViewModelValidator = customerSettingsViewModelValidator;
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
        [ActionName("GetCustomerSettings")]
        public async Task<IActionResult> Get()
        {
            var currentUserResponse = await this.currentUser.GetCurrentUserFromCache();
            var customerId = currentUserResponse.Result.Id;

            var customerSettingsResponse = await this.customerSettingsBusiness.GetCustomerSettings(customerId);

            if (customerSettingsResponse.ResultType == ResultType.Success)
            {
                var customerSettingsViewModelResponse = new Response<CustomerSettingsViewModel>()
                { Result = CustomerSettingsViewModel.CreateCustomerSettingsViewModel(customerSettingsResponse.Result) };
                return this.CreateGetHttpResponse(customerSettingsViewModelResponse);
            }

            return this.CreateGetHttpResponse(new Response<CustomerSettingsViewModel>()
            { ResultType = customerSettingsResponse.ResultType, Messages = customerSettingsResponse.Messages });
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
        [ActionName("UpdateCustomerSettings")]
        public async Task<IActionResult> Put([FromBody] CustomerSettingsViewModel customerSettingsViewModel)
        {
            ValidationResult results = this.customerSettingsViewModelValidator.Validate(customerSettingsViewModel);

            if (results.IsValid)
            {
                var currentUserResponse = await this.currentUser.GetCurrentUserFromCache();
                var customerId = currentUserResponse.Result.Id;
                var customerSettingsResponse = await this.customerSettingsBusiness.UpdateCustomerSettings(customerId, CreateCustomerSettingsViewModel(customerSettingsViewModel));
                return this.CreatePutHttpResponse(customerSettingsResponse);
            }
            var validationResponse = Response<bool>.ValidationError(results.Errors.Select(a => a.ErrorMessage).ToList());
            return this.CreatePutHttpResponse(validationResponse);
        }

        private CustomerSettingsModel CreateCustomerSettingsViewModel(CustomerSettingsViewModel customerSettingsViewModel)
        {
            return new CustomerSettingsModel() { Country = customerSettingsViewModel.Country, TimeZone = customerSettingsViewModel.TimeZone };
        }
    }
}
