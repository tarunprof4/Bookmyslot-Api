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
    public class CustomerLastSharedSlotController : BaseApiController
    {
        private readonly ICustomerLastSharedSlotBusiness customerLastSharedSlotBusiness;
        private readonly ICurrentUser currentUser;

        public CustomerLastSharedSlotController(ICustomerLastSharedSlotBusiness customerLastSharedSlotBusiness, ICurrentUser currentUser)
        {
            this.customerLastSharedSlotBusiness = customerLastSharedSlotBusiness;
            this.currentUser = currentUser;
        }

        /// <summary>
        /// Gets customer latest shared slot
        /// </summary>
        /// <returns>returns customer latest slot</returns>
        /// <response code="200">Returns customer latest slot information</response>
        /// <response code="404">no slots found</response>
        /// <response code="500">internal server error</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            var currentUserResponse = await this.currentUser.GetCurrentUserFromCache();
            var customerId = currentUserResponse.Result;
            var customerLastSlotResponse = await this.customerLastSharedSlotBusiness.GetCustomerLatestSharedSlot(customerId);
            return this.CreateGetHttpResponse(customerLastSlotResponse);
        }


    }



    
}
