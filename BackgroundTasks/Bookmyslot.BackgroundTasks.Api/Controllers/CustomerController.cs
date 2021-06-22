using Bookmyslot.Api.Web.Common;
using Bookmyslot.BackgroundTasks.Api.Contracts;
using Bookmyslot.BackgroundTasks.Api.Contracts.Interfaces.Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Bookmyslot.BackgroundTasks.Api.Controllers
{
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    public class CustomerController : BaseApiController
    {
        private readonly ICustomerBusiness customerBusiness;
        public CustomerController(ICustomerBusiness customerBusiness)
        {
            this.customerBusiness = customerBusiness;
        }


        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("api/v1/Customer")]
        [HttpPost()]
        [ActionName("CreateCustomer")]
        public async Task<IActionResult> Post()
        {
            var customerModel = GetSearchCustomerModel();

            var createCustomerResponse = await this.customerBusiness.CreateCustomer(customerModel);

            return this.CreatePostHttpResponse(createCustomerResponse);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("api/v1/Customer")]
        [HttpPut()]
        [ActionName("UpdateCustomer")]
        public async Task<IActionResult> Put()
        {
            var customerModel = GetSearchCustomerModel();

            var updateCustomerResponse = await this.customerBusiness.UpdateCustomer(customerModel);

            return this.CreatePostHttpResponse(updateCustomerResponse);
        }

        private CustomerModel GetSearchCustomerModel()
        {
            return new CustomerModel()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "UserName",
                FirstName = "First",

                LastName = "Last",
                Email = "tarun.aggarwal4@gmail.com",
                BioHeadLine = "BioHea"
            };
        }




    }
}
