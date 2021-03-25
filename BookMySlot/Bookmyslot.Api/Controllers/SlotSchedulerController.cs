using Bookmyslot.Api.Authentication.Common.Interfaces;
using Bookmyslot.Api.Common.Compression.Interfaces;
using Bookmyslot.Api.Common.Contracts;
using Bookmyslot.Api.Common.Contracts.Constants;
using Bookmyslot.Api.SlotScheduler.Contracts;
using Bookmyslot.Api.SlotScheduler.Contracts.Interfaces;
using Bookmyslot.Api.SlotScheduler.ViewModels;
using Bookmyslot.Api.SlotScheduler.ViewModels.Validations;
using Bookmyslot.Api.Web.Common;
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
        private readonly IKeyEncryptor keyEncryptor;
        private readonly ICurrentUser currentUser;
        public SlotSchedulerController(ISlotSchedulerBusiness slotSchedulerBusiness, IKeyEncryptor keyEncryptor, ICurrentUser currentUser)
        {
            this.slotSchedulerBusiness = slotSchedulerBusiness;
            this.keyEncryptor = keyEncryptor;
            this.currentUser = currentUser;
        }


        [Route("api/v1/SlotScheduler")]
        [HttpPost]
        [ActionName("ScheduleSlot")]
        public async Task<IActionResult> Post([FromBody] SlotSchedulerViewModel slotSchedulerViewModel)
        {
            var validator = new SlotSchedulerViewModelValidator();
            ValidationResult results = validator.Validate(slotSchedulerViewModel);

            if (results.IsValid)
            {
                var customerSlotModel = JsonConvert.DeserializeObject<SlotModel>(this.keyEncryptor.Decrypt(slotSchedulerViewModel.SlotModelKey));

                if (customerSlotModel != null)
                {
                    var currentUserResponse = await this.currentUser.GetCurrentUserFromCache();
                    var customerId = currentUserResponse.Result.Id;
                    var slotScheduleResponse = await this.slotSchedulerBusiness.ScheduleSlot(customerSlotModel, customerId);
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
