using Bookmyslot.Api.Customers.Contracts;
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
    public class RegisterCustomerController : BaseApiController
    {
        private readonly IRegisterCustomerBusiness registerCustomerBusiness;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterCustomerController"/> class. 
        /// </summary>
        /// <param name="registerCustomerBusiness">register customer Business</param>
        public RegisterCustomerController(IRegisterCustomerBusiness registerCustomerBusiness)
        {
            this.registerCustomerBusiness = registerCustomerBusiness;
        }


        /// <summary>
        /// Create new customer
        /// </summary>
        /// <param name="registerCustomerModel">register customer model</param>
        /// <returns >returns email id of created customer</returns>
        /// <response code="201">Returns email id of created customer</response>
        /// <response code="400">validation error bad request</response>
        /// <response code="500">internal server error</response>
        // POST api/<CustomerController>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RegisterCustomerModel registerCustomerModel)
        {
            var customerResponse = await registerCustomerBusiness.RegisterCustomer(registerCustomerModel);
            return this.CreatePostHttpResponse(customerResponse);
        }
    }
}
