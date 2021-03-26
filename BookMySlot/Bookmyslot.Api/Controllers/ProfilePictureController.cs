using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
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
    [ApiController]
    [Authorize]
    public class ProfilePictureController : BaseApiController
    {
        private const string Image = "image";
        private readonly IProfileSettingsBusiness profileSettingsBusiness;
        private readonly ICurrentUser currentUser;
        private readonly IFileConfigurationBusiness fileConfigurationBusiness;



        /// <summary>
        /// Initializes a new instance of the <see cref="ProfilePictureController"/> class. 
        /// </summary>
        /// <param name="profileSettingsBusiness">profileSettings Business</param>
        /// <param name="currentUser">currentUser</param>
        /// <param name="fileConfigurationBusiness">fileConfigurationBusiness</param>

        public ProfilePictureController(IProfileSettingsBusiness profileSettingsBusiness, ICurrentUser currentUser, IFileConfigurationBusiness fileConfigurationBusiness)
        {
            this.profileSettingsBusiness = profileSettingsBusiness;
            this.currentUser = currentUser;
            this.fileConfigurationBusiness = fileConfigurationBusiness;
        }

        /// <summary>
        /// Update customer profile picture
        /// </summary>
        /// <param name="file">formFile model</param>
        /// <returns>success or failure bool</returns>
        /// <response code="204">Returns success or failure bool</response>
        /// <response code="400">validation error bad request</response>
        /// <response code="401">unauthorized user</response>
        /// <response code="500">internal server error</response>
        // PUT api/<CustomerController>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut]
        [ActionName("UpdateProfilePicture")]
        [Route("api/v1/ProfilePicture/UpdateProfilePicture")]
        public async Task<IActionResult> UpdateProfilePicture([FromForm(Name = Image)] IFormFile file)
        {
            var validator = new UpdateProfilePictureViewModelValidator(this.fileConfigurationBusiness);
            ValidationResult results = validator.Validate(new UpdateProfilePictureViewModel(file));

            if (results.IsValid)
            {
                var currentUserResponse = await this.currentUser.GetCurrentUserFromCache();
                var customerId = currentUserResponse.Result.Id;
                var firstName = currentUserResponse.Result.FirstName;
                var profileUpdateResponse = await this.profileSettingsBusiness.UpdateProfilePicture(file, customerId, firstName);
                return this.CreatePutHttpResponse(profileUpdateResponse);
            }

            var validationResponse = Response<string>.ValidationError(results.Errors.Select(a => a.ErrorMessage).ToList());
            return this.CreatePutHttpResponse(validationResponse);
        }
    }
}
