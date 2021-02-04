using Bookmyslot.Api.Common;
using Bookmyslot.Api.Customers.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.Search.Contracts.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bookmyslot.Api.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    public class SearchCustomerController : BaseApiController
    {
        private readonly ISearchCustomerBusiness searchCustomerBusiness;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchCustomerController"/> class. 
        /// </summary>
        /// <param name="searchCustomerBusiness">search customer Business</param>
        public SearchCustomerController(ISearchCustomerBusiness searchCustomerBusiness)
        {
            this.searchCustomerBusiness = searchCustomerBusiness;
        }


        /// <summary>
        /// Gets customer by search 
        /// </summary>
        /// <param name="searchKey">search key</param>
        /// <returns >customer details</returns>
        /// <response code="200">Returns searched customers</response>
        /// <response code="404">no customer found</response>
        /// <response code="400">validation error bad request</response>
        /// <response code="500">internal server error</response>
        // GET api/<CustomerController>/email
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{email}")]
        public async Task<IActionResult> Get(string searchKey)
        {
            var customerResponse = await searchCustomerBusiness.SearchCustomers(searchKey);
            return this.CreateGetHttpResponse(customerResponse);
        }
    }
}
