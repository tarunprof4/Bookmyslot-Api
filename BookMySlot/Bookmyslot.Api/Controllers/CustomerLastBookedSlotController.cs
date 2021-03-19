using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces.Business;
using Bookmyslot.Api.Web.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Bookmyslot.Api.Controllers
{
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    [Authorize]
    public class CustomerLastBookedSlotController : BaseApiController
    {
        private readonly ICustomerLastBookedSlotBusiness customerLastBookedSlotBusiness;
        private readonly ICurrentUser currentUser;

        public CustomerLastBookedSlotController(ICustomerLastBookedSlotBusiness customerLastBookedSlotBusiness, ICurrentUser currentUser)
        {
            this.customerLastBookedSlotBusiness = customerLastBookedSlotBusiness;
            this.currentUser = currentUser;
        }

        /// <summary>
        /// Gets customer lastes slot
        /// </summary>
        /// <returns>returns customer latest slot</returns>
        /// <response code="200">Returns customer latest slot information</response>
        /// <response code="404">no slots found</response>
        /// <response code="500">internal server error</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("api/v1/CustomerLastBookedSlot/GetCustomerLastSlot")]
        [HttpGet()]
        public async Task<IActionResult> GetCustomerLastSlot()
        {
            var currentUserResponse = await this.currentUser.GetCurrentUserFromCache();
            var customerId = currentUserResponse.Result;
            var customerLastSlotResponse = await this.customerLastBookedSlotBusiness.GetCustomerLatestSlot(customerId);
            return this.CreateGetHttpResponse(customerLastSlotResponse);
        }


    }



    
}
