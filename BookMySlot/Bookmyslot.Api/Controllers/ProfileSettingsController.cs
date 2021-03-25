using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.Customers.ViewModels;
using Bookmyslot.Api.Customers.ViewModels.Validations;
using Bookmyslot.Api.File.Contracts.Interfaces;
using Bookmyslot.Api.File.ViewModels;
using Bookmyslot.Api.File.ViewModels.Validations;
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
    public class ProfileSettingsController : BaseApiController
    {
        private const string Image = "image";
        private readonly IProfileSettingsBusiness profileSettingsBusiness;
        private readonly ICurrentUser currentUser;
        private readonly IFileConfigurationBusiness fileConfigurationBusiness;



        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileSettingsController"/> class. 
        /// </summary>
        /// <param name="profileSettingsBusiness">profileSettings Business</param>
        /// <param name="currentUser">currentUser</param>

        public ProfileSettingsController(IProfileSettingsBusiness profileSettingsBusiness, ICurrentUser currentUser, IFileConfigurationBusiness fileConfigurationBusiness)
        {
            this.profileSettingsBusiness = profileSettingsBusiness;
            this.currentUser = currentUser;
            this.fileConfigurationBusiness = fileConfigurationBusiness;
            
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
            var customerId = currentUserResponse.Result;

            var customerResponse = await this.profileSettingsBusiness.GetProfileSettingsByCustomerId(customerId);
            return this.CreateGetHttpResponse(customerResponse);
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
            var validator = new ProfileSettingsViewModelValidator();
            ValidationResult results = validator.Validate(profileSettingsViewModel);

            if (results.IsValid)
            {
                var currentUserResponse = await this.currentUser.GetCurrentUserFromCache();
                var customerId = currentUserResponse.Result;
                var customerResponse = await this.profileSettingsBusiness.UpdateProfileSettings(CreateProfileSettingsModel(profileSettingsViewModel), customerId);
                return this.CreatePutHttpResponse(customerResponse);
            }
            var validationResponse = Response<bool>.ValidationError(results.Errors.Select(a => a.ErrorMessage).ToList());
            return this.CreatePutHttpResponse(validationResponse);
        }



        /// <summary>
        /// Update customer profile picture
        /// </summary>
        /// <param name="file">formFile model</param>
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
        [ActionName("UpdateProfilePicture")]
        [Route("api/v1/ProfileSettings/UpdateProfilePicture")]
        public async Task<IActionResult> UpdateProfilePicture([FromForm(Name = Image)] IFormFile file)
        {
            var validator = new UpdateProfilePictureViewModelValidator(this.fileConfigurationBusiness);
            ValidationResult results = validator.Validate(new UpdateProfilePictureViewModel(file));

            if (results.IsValid)
            {
                var currentUserResponse = await this.currentUser.GetCurrentUserFromCache();
                var customerId = currentUserResponse.Result;

                var profileUpdateResponse = await this.profileSettingsBusiness.UpdateProfilePicture(file, customerId);
                return this.CreatePutHttpResponse(profileUpdateResponse);
            }

            var validationResponse = Response<string>.ValidationError(results.Errors.Select(a => a.ErrorMessage).ToList());
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
