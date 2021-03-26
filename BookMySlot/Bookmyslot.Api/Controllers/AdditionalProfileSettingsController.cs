﻿using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.Customers.ViewModels;
using Bookmyslot.Api.Customers.ViewModels.Validations;
using Bookmyslot.Api.Web.Common;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Controllers
{

    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    [Authorize]
    public class AdditionalProfileSettingsController : BaseApiController
    {
        private readonly IAdditionalProfileSettingsBusiness additionalProfileSettingsBusiness;
        private readonly ICurrentUser currentUser;

        public AdditionalProfileSettingsController(IAdditionalProfileSettingsBusiness additionalProfileSettingsBusiness, ICurrentUser currentUser)
        {
            this.additionalProfileSettingsBusiness = additionalProfileSettingsBusiness;
            this.currentUser = currentUser;
        }


        /// <summary>
        /// Gets profile settings by email
        /// </summary>
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
        [HttpGet]
        [ActionName("GetProfileSettings")]
        [Route("api/v1/AdditionalProfileSettings")]
        public async Task<IActionResult> Get()
        {
            var currentUserResponse = await this.currentUser.GetCurrentUserFromCache();

            var additionalProfileSettingsModel = new Response<AdditionalProfileSettingsModel>() { Result = new AdditionalProfileSettingsModel() { BioHeadLine = currentUserResponse.Result.BioHeadLine } };
            return this.CreateGetHttpResponse(additionalProfileSettingsModel);
        }



        /// <summary>
        /// Update existing customer
        /// </summary>
        /// <param name="additionalProfileSettingsViewModel">profileSettings model</param>
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
        [Route("api/v1/AdditionalProfileSettings")]
        public async Task<IActionResult> Put([FromBody] AdditionalProfileSettingsViewModel additionalProfileSettingsViewModel)
        {
            var validator = new AdditionalProfileSettingsViewModelValidator();
            ValidationResult results = validator.Validate(additionalProfileSettingsViewModel);

            if (results.IsValid)
            {
                var currentUserResponse = await this.currentUser.GetCurrentUserFromCache();
                var customerId = currentUserResponse.Result.Id;

                var customerResponse = await this.additionalProfileSettingsBusiness.UpdateAdditionalProfileSettings(customerId, CreateAdditionalProfileSettingsModel(additionalProfileSettingsViewModel));
                return this.CreatePutHttpResponse(customerResponse);
            }
            var validationResponse = Response<bool>.ValidationError(results.Errors.Select(a => a.ErrorMessage).ToList());
            return this.CreatePutHttpResponse(validationResponse);
        }



        private AdditionalProfileSettingsModel CreateAdditionalProfileSettingsModel(AdditionalProfileSettingsViewModel additionalProfileSettingsViewModel)
        {
            return new AdditionalProfileSettingsModel() { BioHeadLine = additionalProfileSettingsViewModel.BioHeadLine };
        }
    }
}
