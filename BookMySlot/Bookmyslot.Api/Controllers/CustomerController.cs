using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Authentication.ViewModels;
using Bookmyslot.Api.Common.Web.Filters;
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
    [ServiceFilter(typeof(AuthorizedFilter))]
    public class CustomerController : BaseApiController
    {
        private readonly ICustomerBusiness customerBusiness;
        private readonly ICurrentUser currentUser;

        public CustomerController(ICustomerBusiness customerBusiness, ICurrentUser currentUser)
        {
            this.customerBusiness = customerBusiness;
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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        [ActionName("GetCustomer")]
        public async Task<IActionResult> Get()
        {
            var currentUserResponse = await this.currentUser.GetCurrentUserFromCache();
            var currentUserViewModelResponse = CurrentUserViewModel.CreateCurrentUserViewModel(currentUserResponse.Value);
            return this.CreateGetHttpResponse(currentUserViewModelResponse);
        }
    }
}
