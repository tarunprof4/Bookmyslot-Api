using Bookmyslot.Api.Common;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Threading.Tasks;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bookmyslot.Api.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    public class CustomerController : BaseApiController
    {
        private readonly ICustomerBusiness customerBusiness;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerController"/> class. 
        /// </summary>
        /// <param name="customerBusiness">customer Business</param>
        public CustomerController(ICustomerBusiness customerBusiness)
        {
            this.customerBusiness = customerBusiness;
        }


        /// <summary>
        /// Gets customer by email
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
        public async Task<IActionResult> Get(string email)
        {
            Log.Information("Get Customer Email " + email);
            var customerResponse = await customerBusiness.GetCustomerByEmail(email);
            return this.CreateGetHttpResponse(customerResponse);
        }

        /// <summary>
        /// Create new customer
        /// </summary>
        /// <param name="customerModel">customer model</param>
        /// <returns >returns email id of created customer</returns>
        /// <response code="201">Returns email id of created customer</response>
        /// <response code="400">validation error bad request</response>
        /// <response code="500">internal server error</response>
        // POST api/<CustomerController>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CustomerModel customerModel)
        {
            Log.Information("Create Customer " + customerModel);
            var customerResponse = await customerBusiness.CreateCustomer(customerModel);
            return this.CreatePostHttpResponse(customerResponse);
        }

        /// <summary>
        /// Update existing customer
        /// </summary>
        /// <param name="customerModel">customer model</param>
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
        public async Task<IActionResult> Put([FromBody] CustomerModel customerModel)
        {
            Log.Information("Update Customer " + customerModel);
            var customerResponse = await customerBusiness.UpdateCustomer(customerModel);
            return this.CreatePutHttpResponse(customerResponse);
        }
    }
}
