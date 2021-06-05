using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.Common.Contracts.Infrastructure.Interfaces.Encryption;
using Bookmyslot.Api.Customers.Domain;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.Domain;
using Bookmyslot.Api.SlotScheduler.ViewModels;
using Bookmyslot.Api.Web.Common;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bookmyslot.Api.Controllers
{
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    [Authorize]
    public class SlotSchedulerController : BaseApiController
    {
        private readonly ISlotSchedulerBusiness slotSchedulerBusiness;
        private readonly ISymmetryEncryption symmetryEncryption;
        private readonly ICurrentUser currentUser;
        private readonly IValidator<SlotSchedulerViewModel> slotSchedulerViewModelValidator;
        public SlotSchedulerController(ISlotSchedulerBusiness slotSchedulerBusiness, ISymmetryEncryption symmetryEncryption,
            ICurrentUser currentUser, IValidator<SlotSchedulerViewModel> slotSchedulerViewModelValidator)
        {
            this.slotSchedulerBusiness = slotSchedulerBusiness;
            this.symmetryEncryption = symmetryEncryption;
            this.currentUser = currentUser;
            this.slotSchedulerViewModelValidator = slotSchedulerViewModelValidator;
        }


        [Route("api/v1/SlotScheduler")]
        [HttpPost]
        [ActionName("ScheduleSlot")]
        public async Task<IActionResult> Post([FromBody] SlotSchedulerViewModel slotSchedulerViewModel)
        {
            ValidationResult results = this.slotSchedulerViewModelValidator.Validate(slotSchedulerViewModel);

            if (results.IsValid)
            {
                var customerSlotModel = JsonConvert.DeserializeObject<SlotModel>(this.symmetryEncryption.Decrypt(slotSchedulerViewModel.SlotModelKey));

                if (customerSlotModel != null)
                {
                    var currentUserResponse = await this.currentUser.GetCurrentUserFromCache();
                    var customerSummaryModel = new CustomerSummaryModel(currentUserResponse.Result);
                    var slotScheduleResponse = await this.slotSchedulerBusiness.ScheduleSlot(customerSlotModel, customerSummaryModel);
                    return this.CreatePostHttpResponse(slotScheduleResponse);
                }

                var validationErrorResponse = Response<bool>.ValidationError(new List<string>() { AppBusinessMessagesConstants.CorruptData });
                return this.CreatePostHttpResponse(validationErrorResponse);
            }

            var validationResponse = Response<bool>.ValidationError(results.Errors.Select(a => a.ErrorMessage).ToList());
            return this.CreatePostHttpResponse(validationResponse);
        }


    }
}
