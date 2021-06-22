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

    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    [Authorize]
    [ServiceFilter(typeof(AuthorizedFilter))]
    public class ProfileSettingsController : BaseApiController
    {
        private readonly IProfileSettingsBusiness profileSettingsBusiness;
        private readonly ICurrentUser currentUser;
        private readonly IValidator<ProfileSettingsViewModel> profileSettingsViewModelValidator;


        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileSettingsController"/> class. 
        /// </summary>
        /// <param name="profileSettingsBusiness">profileSettings Business</param>
        /// <param name="currentUser">currentUser</param>

        public ProfileSettingsController(IProfileSettingsBusiness profileSettingsBusiness, ICurrentUser currentUser, IValidator<ProfileSettingsViewModel> profileSettingsViewModelValidator)
        {
            this.profileSettingsBusiness = profileSettingsBusiness;
            this.currentUser = currentUser;
            this.profileSettingsViewModelValidator = profileSettingsViewModelValidator;
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
        [Route("api/v1/ProfileSettings")]
        public async Task<IActionResult> Get()
        {
            var currentUserResponse = await this.currentUser.GetCurrentUserFromCache();
            var customerId = currentUserResponse.Result.Id;
            var profileSettingsResponse = await this.profileSettingsBusiness.GetProfileSettingsByCustomerId(customerId);

            if (profileSettingsResponse.ResultType == ResultType.Success)
            {
                var profileSettingsViewModelResponse = new Response<ProfileSettingsViewModel>()
                { Result = ProfileSettingsViewModel.CreateProfileSettingsViewModel(profileSettingsResponse.Result) };
                return this.CreateGetHttpResponse(profileSettingsViewModelResponse);
            }

            return this.CreateGetHttpResponse(new Response<ProfileSettingsViewModel>()
            { ResultType = profileSettingsResponse.ResultType, Messages = profileSettingsResponse.Messages });
        }



        /// <summary>
        /// Update existing customer
        /// </summary>
        /// <param name="profileSettingsViewModel">profileSettings model</param>
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
        [Route("api/v1/ProfileSettings")]
        public async Task<IActionResult> Put([FromBody] ProfileSettingsViewModel profileSettingsViewModel)
        {
            ValidationResult results = this.profileSettingsViewModelValidator.Validate(profileSettingsViewModel);

            if (results.IsValid)
            {
                var currentUserResponse = await this.currentUser.GetCurrentUserFromCache();
                var customerId = currentUserResponse.Result.Id;

                var customerResponse = await this.profileSettingsBusiness.UpdateProfileSettings(CreateProfileSettingsModel(profileSettingsViewModel), customerId);
                return this.CreatePutHttpResponse(customerResponse);
            }
            var validationResponse = Response<bool>.ValidationError(results.Errors.Select(a => a.ErrorMessage).ToList());
            return this.CreatePutHttpResponse(validationResponse);
        }



        private ProfileSettingsModel CreateProfileSettingsModel(ProfileSettingsViewModel profileSettingsViewModel)
        {
            var profileSettingsModel = new ProfileSettingsModel();
            profileSettingsModel.FirstName = profileSettingsViewModel.FirstName;
            profileSettingsModel.LastName = profileSettingsViewModel.LastName;
            profileSettingsModel.Gender = profileSettingsViewModel.Gender;
            return profileSettingsModel;
        }





    }
}
