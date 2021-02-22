using Bookmyslot.Api.Authentication.Common;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.Web.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Controllers
{
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    public class LoginController : BaseApiController
    {
        private readonly ILoginCustomerBusiness loginCustomerBusiness;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginController"/> class. 
        /// </summary>
        public LoginController(ILoginCustomerBusiness loginCustomerBusiness)
        {
            this.loginCustomerBusiness = loginCustomerBusiness;
        }


        /// <summary>
        /// Create new customer
        /// </summary>
        /// <param name="socialCustomerModel">social customer model</param>
        /// <returns >returns access token for customer</returns>
        /// <response code="201">Returns access token</response>
        /// <response code="400">validation error bad request</response>
        /// <response code="500">internal server error</response>
        // POST api/<CustomerController>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [ActionName("LoginSocialUser")]
        [Route("api/v1/Login/SocialCustomerLogin")]
        public async Task<IActionResult> SocialCustomerLogin([FromBody] SocialCustomerModel socialCustomerModel)
        {
            var loginResponse = await this.loginCustomerBusiness.LoginSocialCustomer(socialCustomerModel);
          
            return this.CreatePostHttpResponse(loginResponse);
        }
    }
}
