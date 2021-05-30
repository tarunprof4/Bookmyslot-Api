using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Customers.Contracts.Interfaces;
using Bookmyslot.Api.Customers.ViewModels;
using Bookmyslot.Api.Web.Common;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
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
        private readonly IValidator<RegisterCustomerViewModel> registerCustomerViewModelValidator;

        public RegisterCustomerController(IRegisterCustomerBusiness registerCustomerBusiness, IValidator<RegisterCustomerViewModel> registerCustomerViewModelValidator)
        {
            this.registerCustomerBusiness = registerCustomerBusiness;
            this.registerCustomerViewModelValidator = registerCustomerViewModelValidator;
        }


        /// <summary>
        /// Create new customer
        /// </summary>
        /// <param name="registerCustomerViewModel">register customer model</param>
        /// <returns >returns email id of created customer</returns>
        /// <response code="201">Returns email id of created customer</response>
        /// <response code="400">validation error bad request</response>
        /// <response code="500">internal server error</response>
        // POST api/<CustomerController>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [ActionName("RegisterCustomer")]
        public async Task<IActionResult> Post([FromBody] RegisterCustomerViewModel registerCustomerViewModel)
        {
            ValidationResult results = this.registerCustomerViewModelValidator.Validate(registerCustomerViewModel);

            if (results.IsValid)
            {
                var customerResponse = await registerCustomerBusiness.RegisterCustomer(CreateRegisterCustomerModel(registerCustomerViewModel));
                return this.CreatePostHttpResponse(customerResponse);
            }

            var validationResponse = Response<string>.ValidationError(results.Errors.Select(a => a.ErrorMessage).ToList());
            return this.CreatePostHttpResponse(validationResponse);
        }

        private RegisterCustomerModel CreateRegisterCustomerModel(RegisterCustomerViewModel registerCustomerViewModel)
        {
            var registerCustomerModel = new RegisterCustomerModel();
            registerCustomerModel.FirstName = registerCustomerViewModel.FirstName;
            registerCustomerModel.LastName = registerCustomerViewModel.LastName;
            registerCustomerModel.Email = registerCustomerViewModel.Email;
            registerCustomerModel.Provider = registerCustomerViewModel.Provider;

            return registerCustomerModel;
        }
    }
}
